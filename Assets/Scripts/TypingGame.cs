// =============================================================================
// TypingGame.cs
// Core game loop managing input, word progression, lives, UI panels, and timer.
// =============================================================================

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class TypingGame : MonoBehaviour
{
    // -------------------------------------------------------------------------
    // Constants
    // -------------------------------------------------------------------------

    private const float BlinkInterval = 0.5f;
    private const float LevelCompleteDelay = 0.8f;
    private const float EnterCooldownDuration = 0.5f;
    private const float CursorMarkPadding = 18f;
    private const string CursorMarkColor = "#FFFFFF50";
    private const string CorrectLetterColor = "#00FF00";
    private const string IncorrectLetterColor = "#FF0000";

    // -------------------------------------------------------------------------
    // Inspector Fields
    // -------------------------------------------------------------------------

    [Header("References")]
    public SubmarineController submarine;
    public CameraFollow cameraFollow;
    public FishSchoolSpawner fishSchoolSpawner;
    public AudioManager audioManager;

    [Header("Word Display")]
    public TMP_Text wordToType;
    public TMP_Text typedWordDisplay;

    [Header("Typing Area")]
    public GameObject backgroundTypingArea;

    [Header("HUD")]
    public GameObject backgroundLevelNumberHearts;
    public GameObject backgroundGameTimer;
    public TMP_Text currentLevelText;

    [Header("Level UI")]
    public GameObject levelStartPanel;
    public TMP_Text levelStartText;
    public float levelStartDisplayTime = 2f;
    public GameObject levelCompletePanel;
    public TMP_Text levelCompleteText;
    public Button nextLevelButton;

    [Header("Lives")]
    public GameObject[] heartObjects;
    public GameObject gameOverPanel;
    public Button newGameButton;

    [Header("Game Won")]
    public GameObject gameWonPanel;
    public TMP_Text gameWonText;
    public Button gameWonNewGameButton;

    [Header("Escape Menu")]
    public GameObject escapeMenuPanel;
    public Slider volumeSlider;
    public Button escapeNewGameButton;
    public Button escapeResumeButton;

    [Header("Progression")]
    public int currentLevel = 1;
    public int wordsPerLevel = 8;
    public int totalLevels = 24;

    // -------------------------------------------------------------------------
    // Private State — Gameplay
    // -------------------------------------------------------------------------

    private string requiredWord = "";
    private string typedInput = "";
    private bool hasError = false;
    private bool gameActive = false;
    private int wordsCompletedThisLevel = 0;
    private float enterCooldown = 0f;
    private List<int> usedWordIndices = new List<int>();

    // -------------------------------------------------------------------------
    // Private State — Lives and Timer
    // -------------------------------------------------------------------------

    private int currentLives = 3;
    private float elapsedTime = 0f;
    private bool timerRunning = false;
    private TMP_Text timerText;

    // -------------------------------------------------------------------------
    // Private State — Escape Menu
    // -------------------------------------------------------------------------

    private bool escapeMenuOpen = false;
    private bool wasGameOverOpen = false;
    private bool wasGameWonOpen = false;
    private bool wasLevelCompleteOpen = false;
    private bool wasLevelStartOpen = false;
    private bool wasTypingAreaOpen = false;

    // -------------------------------------------------------------------------
    // Private State — Coroutines and Blink
    // -------------------------------------------------------------------------

    private Coroutine levelStartCoroutine;
    private Coroutine blinkCoroutine;
    private bool blinkVisible = true;

    // -------------------------------------------------------------------------
    // Unity Lifecycle
    // -------------------------------------------------------------------------

    // Hide typing area before first frame
    void Awake()
    {
        if (backgroundTypingArea != null)
        {
            backgroundTypingArea.SetActive(false);
        }
    }

    // Register text input callback
    void OnEnable()
    {
        Keyboard.current.onTextInput += OnTextInput;
    }

    // Unregister text input callback
    void OnDisable()
    {
        Keyboard.current.onTextInput -= OnTextInput;
    }

    // Wire up buttons and sliders then kick off the first level
    void Start()
    {
        Application.targetFrameRate = 60;

        if (backgroundGameTimer != null)
        {
            timerText = backgroundGameTimer.GetComponentInChildren<TMP_Text>();
        }

        if (nextLevelButton != null)
        {
            nextLevelButton.onClick.AddListener(OnNextLevelPressed);
        }

        if (newGameButton != null)
        {
            newGameButton.onClick.AddListener(OnNewGamePressed);
        }

        if (gameWonNewGameButton != null)
        {
            gameWonNewGameButton.onClick.AddListener(OnNewGamePressed);
        }

        if (escapeMenuPanel != null)
        {
            escapeMenuPanel.SetActive(false);
        }

        if (escapeResumeButton != null)
        {
            escapeResumeButton.onClick.AddListener(CloseEscapeMenu);
        }

        if (escapeNewGameButton != null)
        {
            escapeNewGameButton.onClick.AddListener(() =>
            {
                CloseEscapeMenu();
                OnNewGamePressed();
            });
        }

        if (volumeSlider != null)
        {
            volumeSlider.minValue = 0f;
            volumeSlider.maxValue = 1f;
            volumeSlider.value = audioManager != null ? audioManager.volume : 1f;
            volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        }

        UpdateHeartsDisplay();

        if (fishSchoolSpawner != null)
        {
            fishSchoolSpawner.SpawnForLevel(currentLevel);
        }

        ShowLevelStart();
    }

    // Tick the timer and route all keyboard input each frame
    void Update()
    {
        UpdateTimer();

        if (enterCooldown > 0f)
        {
            enterCooldown -= Time.deltaTime;
        }

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (escapeMenuOpen)
            {
                CloseEscapeMenu();
            }
            else
            {
                OpenEscapeMenu();
            }

            return;
        }

        HandleEnterSubmit();

        if (!gameActive || escapeMenuOpen)
        {
            return;
        }

        HandleBackspace();
        HandleTabCycling();
    }

    // -------------------------------------------------------------------------
    // Escape Menu
    // -------------------------------------------------------------------------

    // Snapshot visible panels, hide gameplay UI, and show the escape menu
    void OpenEscapeMenu()
    {
        escapeMenuOpen = true;

        if (levelStartCoroutine != null)
        {
            StopCoroutine(levelStartCoroutine);
            levelStartCoroutine = null;
        }

        wasGameOverOpen = gameOverPanel != null && gameOverPanel.activeSelf;
        wasGameWonOpen = gameWonPanel != null && gameWonPanel.activeSelf;
        wasLevelCompleteOpen = levelCompletePanel != null && levelCompletePanel.activeSelf;
        wasLevelStartOpen = levelStartPanel != null && levelStartPanel.activeSelf;
        wasTypingAreaOpen = backgroundTypingArea != null && backgroundTypingArea.activeSelf;

        typedInput = "";
        hasError = false;

        StopBlink();
        UpdateTypingDisplay();

        if (backgroundLevelNumberHearts != null)
        {
            backgroundLevelNumberHearts.SetActive(false);
        }

        if (backgroundTypingArea != null)
        {
            backgroundTypingArea.SetActive(false);
        }

        if (backgroundGameTimer != null)
        {
            backgroundGameTimer.SetActive(false);
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        if (gameWonPanel != null)
        {
            gameWonPanel.SetActive(false);
        }

        if (levelCompletePanel != null)
        {
            levelCompletePanel.SetActive(false);
        }

        if (levelStartPanel != null)
        {
            levelStartPanel.SetActive(false);
        }

        if (escapeMenuPanel != null)
        {
            escapeMenuPanel.SetActive(true);
        }

        StopTimer();
    }

    // Restore all panels to their pre-escape state and resume gameplay
    void CloseEscapeMenu()
    {
        escapeMenuOpen = false;

        if (escapeMenuPanel != null)
        {
            escapeMenuPanel.SetActive(false);
        }

        if (backgroundLevelNumberHearts != null)
        {
            backgroundLevelNumberHearts.SetActive(true);
        }

        if (backgroundGameTimer != null)
        {
            backgroundGameTimer.SetActive(true);
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(wasGameOverOpen);
        }

        if (gameWonPanel != null)
        {
            gameWonPanel.SetActive(wasGameWonOpen);
        }

        if (levelCompletePanel != null)
        {
            levelCompletePanel.SetActive(wasLevelCompleteOpen);
        }

        if (levelStartPanel != null)
        {
            levelStartPanel.SetActive(wasLevelStartOpen);
        }

        if (backgroundTypingArea != null)
        {
            backgroundTypingArea.SetActive(wasTypingAreaOpen);
        }

        if (wasLevelStartOpen)
        {
            levelStartCoroutine = StartCoroutine(LevelStartCountdown());
        }
        else if (gameActive && !wasGameOverOpen && !wasGameWonOpen && !wasLevelCompleteOpen)
        {
            StartTimer();

            if (wasTypingAreaOpen)
            {
                StartBlink();
            }
        }

        if (wasGameOverOpen || wasGameWonOpen)
        {
            if (backgroundLevelNumberHearts != null)
            {
                backgroundLevelNumberHearts.SetActive(false);
            }

            if (backgroundGameTimer != null)
            {
                backgroundGameTimer.SetActive(false);
            }
        }
    }

    // Forward volume slider changes to the audio manager
    void OnVolumeChanged(float newVolume)
    {
        if (audioManager != null)
        {
            audioManager.SetVolume(newVolume);
        }
    }

    // -------------------------------------------------------------------------
    // Level Flow
    // -------------------------------------------------------------------------

    // Wait for the display duration then begin the level
    IEnumerator LevelStartCountdown()
    {
        yield return new WaitForSeconds(levelStartDisplayTime);
        levelStartCoroutine = null;
        BeginLevel();
    }

    // Show the level start panel and kick off the countdown
    void ShowLevelStart()
    {
        gameActive = false;

        StopTimer();
        HideTypingArea();

        if (levelStartPanel != null)
        {
            levelStartPanel.SetActive(true);
        }

        if (levelStartText != null)
        {
            levelStartText.text = $"Level {currentLevel}";
        }

        if (levelCompletePanel != null)
        {
            levelCompletePanel.SetActive(false);
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        if (gameWonPanel != null)
        {
            gameWonPanel.SetActive(false);
        }

        if (currentLevelText != null)
        {
            currentLevelText.text = $"Level {currentLevel}";
        }

        if (levelStartCoroutine != null)
        {
            StopCoroutine(levelStartCoroutine);
        }

        levelStartCoroutine = StartCoroutine(LevelStartCountdown());
    }

    // Hide the start panel and begin active gameplay
    void BeginLevel()
    {
        if (levelStartPanel != null)
        {
            levelStartPanel.SetActive(false);
        }

        gameActive = true;

        StartTimer();
        PickNewWord();
    }

    // Wait for the sub to settle, run the victory animation, then show the end panel
    IEnumerator LevelCompleteSequence()
    {
        yield return new WaitForSeconds(LevelCompleteDelay);

        StopTimer();

        if (cameraFollow != null)
        {
            cameraFollow.SetFollowEnabled(false);
        }

        if (submarine != null)
        {
            float centerX = cameraFollow != null ? cameraFollow.transform.position.x : 0f;

            yield return submarine.PlayVictoryFlip(centerX);

            submarine.StartVictoryLoop();
        }

        if (currentLevel >= totalLevels)
        {
            ShowGameWon();
        }
        else
        {
            ShowLevelComplete();
        }
    }

    // Show the level complete panel with the current level number
    void ShowLevelComplete()
    {
        if (levelCompleteText != null)
        {
            levelCompleteText.text = $"Level {currentLevel} Complete!";
        }

        if (levelCompletePanel != null)
        {
            levelCompletePanel.SetActive(true);
        }
    }

    // Advance to the next level and reset per-level state
    void OnNextLevelPressed()
    {
        currentLevel++;
        wordsCompletedThisLevel = 0;
        usedWordIndices.Clear();

        if (levelCompletePanel != null)
        {
            levelCompletePanel.SetActive(false);
        }

        if (submarine != null)
        {
            submarine.StopVictory();
            submarine.ResetToCenter();
        }

        if (cameraFollow != null)
        {
            cameraFollow.ResetToStart();
        }

        if (fishSchoolSpawner != null)
        {
            fishSchoolSpawner.SpawnForLevel(currentLevel);
        }

        if (currentLevel > totalLevels)
        {
            ShowGameWon();
            return;
        }

        ShowLevelStart();
    }

    // Reset all game state and restart from level 1
    void OnNewGamePressed()
    {
        currentLives = 3;
        currentLevel = 1;
        elapsedTime = 0f;
        wordsCompletedThisLevel = 0;
        usedWordIndices.Clear();

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        if (gameWonPanel != null)
        {
            gameWonPanel.SetActive(false);
        }

        if (backgroundLevelNumberHearts != null)
        {
            backgroundLevelNumberHearts.SetActive(true);
        }

        if (backgroundGameTimer != null)
        {
            backgroundGameTimer.SetActive(true);
        }

        if (submarine != null)
        {
            submarine.ResetToCenter();
        }

        if (cameraFollow != null)
        {
            cameraFollow.ResetToStart();
        }

        if (fishSchoolSpawner != null)
        {
            fishSchoolSpawner.SpawnForLevel(currentLevel);
        }

        UpdateHeartsDisplay();
        ShowLevelStart();
    }

    // -------------------------------------------------------------------------
    // Lives and Game Over
    // -------------------------------------------------------------------------

    // Decrement lives and trigger game over if none remain
    public void OnHitFish()
    {
        if (!gameActive)
        {
            return;
        }

        currentLives--;
        UpdateHeartsDisplay();

        if (currentLives <= 0)
        {
            ShowGameOver();
        }
    }

    // Sync heart GameObjects to the current lives count
    void UpdateHeartsDisplay()
    {
        if (heartObjects == null)
        {
            return;
        }

        for (int index = 0; index < heartObjects.Length; index++)
        {
            if (heartObjects[index] != null)
            {
                heartObjects[index].SetActive(index < currentLives);
            }
        }
    }

    // End the game and show the game over panel
    void ShowGameOver()
    {
        gameActive = false;

        StopTimer();
        HideTypingArea();

        if (backgroundLevelNumberHearts != null)
        {
            backgroundLevelNumberHearts.SetActive(false);
        }

        if (backgroundGameTimer != null)
        {
            backgroundGameTimer.SetActive(false);
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
    }

    // End the game and show the win panel with final time and remaining lives
    void ShowGameWon()
    {
        gameActive = false;

        StopTimer();
        HideTypingArea();

        if (backgroundLevelNumberHearts != null)
        {
            backgroundLevelNumberHearts.SetActive(false);
        }

        if (backgroundGameTimer != null)
        {
            backgroundGameTimer.SetActive(false);
        }

        if (gameWonText != null)
        {
            int minutes = (int)(elapsedTime / 60);
            int seconds = (int)(elapsedTime % 60);
            int centiseconds = (int)(elapsedTime * 100 % 100);

            gameWonText.text = $"You Win!\n\nTime: {minutes:D2}:{seconds:D2}.{centiseconds:D2}\nLives Remaining: {currentLives}";
        }

        if (gameWonPanel != null)
        {
            gameWonPanel.SetActive(true);
        }
    }

    // -------------------------------------------------------------------------
    // Timer
    // -------------------------------------------------------------------------

    // Begin accumulating elapsed time
    void StartTimer()
    {
        timerRunning = true;
    }

    // Stop accumulating elapsed time
    void StopTimer()
    {
        timerRunning = false;
    }

    // Accumulate elapsed time and update the timer display
    void UpdateTimer()
    {
        if (timerRunning)
        {
            elapsedTime += Time.deltaTime;
        }

        if (timerText == null)
        {
            return;
        }

        int minutes = (int)(elapsedTime / 60);
        int seconds = (int)(elapsedTime % 60);
        int centiseconds = (int)(elapsedTime * 100 % 100);

        timerText.text = $"{minutes:D2}:{seconds:D2}.{centiseconds:D2}";
    }

    // -------------------------------------------------------------------------
    // Input Handling
    // -------------------------------------------------------------------------

    // Delete the last typed character and refresh the display
    void HandleBackspace()
    {
        if (!Keyboard.current.backspaceKey.wasPressedThisFrame)
        {
            return;
        }

        if (typedInput.Length > 0)
        {
            typedInput = typedInput.Substring(0, typedInput.Length - 1);
            hasError = false;

            StartBlink();
            UpdateTypingDisplay();
        }
    }

    // Cycle the submarine to the next row
    void HandleTabCycling()
    {
        if (!Keyboard.current.tabKey.wasPressedThisFrame)
        {
            return;
        }

        if (submarine == null)
        {
            return;
        }

        submarine.CycleDown();
        enterCooldown = EnterCooldownDuration;
    }

    // Route enter presses to the correct action based on which panel is active
    void HandleEnterSubmit()
    {
        if (!Keyboard.current.enterKey.wasPressedThisFrame)
        {
            return;
        }

        if (escapeMenuOpen)
        {
            return;
        }

        if (gameOverPanel != null && gameOverPanel.activeSelf)
        {
            OnNewGamePressed();
            return;
        }

        if (gameWonPanel != null && gameWonPanel.activeSelf)
        {
            OnNewGamePressed();
            return;
        }

        if (levelCompletePanel != null && levelCompletePanel.activeSelf)
        {
            OnNextLevelPressed();
            return;
        }

        if (gameActive && enterCooldown <= 0f)
        {
            SubmitWord();
        }
    }

    // Append a valid character to the typed input and check for correctness
    void OnTextInput(char letter)
    {
        if (!gameActive)
        {
            return;
        }

        if (escapeMenuOpen)
        {
            return;
        }

        if (hasError)
        {
            return;
        }

        if (letter == '\t' || letter == '\b' || letter == '\n' || letter == '\r')
        {
            return;
        }

        if (typedInput.Length >= requiredWord.Length)
        {
            return;
        }

        if (letter == ' ' && requiredWord[typedInput.Length] != ' ')
        {
            return;
        }

        if (wordToType != null && !wordToType.font.HasCharacter(letter, true))
        {
            return;
        }

        typedInput += letter;

        int lastIndex = typedInput.Length - 1;
        bool isCorrect = lastIndex < requiredWord.Length && typedInput[lastIndex] == requiredWord[lastIndex];

        if (!isCorrect)
        {
            hasError = true;
        }

        StartBlink();
        UpdateTypingDisplay();
    }

    // -------------------------------------------------------------------------
    // Word Management
    // -------------------------------------------------------------------------

    // Pick a random unused word, reset input state, and show the typing area
    void PickNewWord()
    {
        string[] currentList = currentLevel switch
        {
            1 => WordLists.Level1,
            2 => WordLists.Level2,
            3 => WordLists.Level3,
            4 => WordLists.Level4,
            5 => WordLists.Level5,
            6 => WordLists.Level6,
            7 => WordLists.Level7,
            8 => WordLists.Level8,
            9 => WordLists.Level9,
            10 => WordLists.Level10,
            11 => WordLists.Level11,
            12 => WordLists.Level12,
            13 => WordLists.Level13,
            14 => WordLists.Level14,
            15 => WordLists.Level15,
            16 => WordLists.Level16,
            17 => WordLists.Level17,
            18 => WordLists.Level18,
            19 => WordLists.Level19,
            20 => WordLists.Level20,
            21 => WordLists.Level21,
            22 => WordLists.Level22,
            23 => WordLists.Level23,
            24 => WordLists.Level24,
            _ => WordLists.Level1
        };

        if (usedWordIndices.Count >= currentList.Length)
        {
            usedWordIndices.Clear();
        }

        int wordIndex;

        do
        {
            wordIndex = Random.Range(0, currentList.Length);
        }
        while (usedWordIndices.Contains(wordIndex));

        usedWordIndices.Add(wordIndex);

        requiredWord = ApplyCapitalization(currentList[wordIndex], currentLevel);
        typedInput = "";
        hasError = false;

        UpdateTypingDisplay();
        StartBlink();
        ShowTypingArea();
    }

    // Apply level-specific capitalisation rules to a word
    string ApplyCapitalization(string word, int level)
    {
        switch (level)
        {
            case 1:
            case 4:
            case 10:
                return Random.value > 0.5f ? word.ToUpper() : word.ToLower();

            case 2:
            case 5:
            case 11:
            case 14:
            case 15:
                if (word.Length == 0)
                {
                    return word;
                }

                return Random.value > 0.5f
                    ? char.ToUpper(word[0]) + word.Substring(1).ToLower()
                    : word.ToLower();

            default:
                return word;
        }
    }

    // Validate typed input and advance to the next word or end the level
    void SubmitWord()
    {
        if (hasError || typedInput.Length == 0)
        {
            return;
        }

        if (typedInput == requiredWord)
        {
            wordsCompletedThisLevel++;

            if (submarine != null)
            {
                submarine.MoveForward();
            }

            if (wordsCompletedThisLevel >= wordsPerLevel)
            {
                wordsCompletedThisLevel = 0;
                gameActive = false;

                HideTypingArea();
                StartCoroutine(LevelCompleteSequence());
            }
            else
            {
                PickNewWord();
            }
        }
        else
        {
            hasError = true;
        }
    }

    // -------------------------------------------------------------------------
    // Typing Display
    // -------------------------------------------------------------------------

    // Hide the typing area panel and stop the cursor blink
    void HideTypingArea()
    {
        StopBlink();

        if (backgroundTypingArea != null)
        {
            backgroundTypingArea.SetActive(false);
        }
    }

    // Show the typing area panel
    void ShowTypingArea()
    {
        if (backgroundTypingArea != null)
        {
            backgroundTypingArea.SetActive(true);
        }
    }

    // Rebuild the target word display with cursor highlight and the typed word display with coloured letters
    void UpdateTypingDisplay()
    {
        if (wordToType != null)
        {
            string targetText = "";

            for (int index = 0; index < requiredWord.Length; index++)
            {
                if (index == typedInput.Length)
                {
                    targetText += blinkVisible
                        ? $"<mark={CursorMarkColor} padding=\"{CursorMarkPadding}, {CursorMarkPadding}, 0, 0\">{requiredWord[index]}</mark>"
                        : requiredWord[index].ToString();
                }
                else
                {
                    targetText += requiredWord[index];
                }
            }

            wordToType.text = targetText;
            wordToType.richText = true;
        }

        if (typedWordDisplay == null)
        {
            return;
        }

        string coloredText = "";

        for (int index = 0; index < typedInput.Length; index++)
        {
            bool isCorrect = index < requiredWord.Length && typedInput[index] == requiredWord[index];
            string color = isCorrect ? CorrectLetterColor : IncorrectLetterColor;
            char displayChar = typedInput[index] == ' ' ? '_' : typedInput[index];

            coloredText += $"<color={color}>{displayChar}</color>";
        }

        typedWordDisplay.text = coloredText;
        typedWordDisplay.richText = true;
    }

    // -------------------------------------------------------------------------
    // Cursor Blink
    // -------------------------------------------------------------------------

    // Cancel any running blink and restart it from the visible state
    void StartBlink()
    {
        StopBlink();
        blinkVisible = true;
        blinkCoroutine = StartCoroutine(BlinkCursor());
    }

    // Stop the blink coroutine and leave the cursor visible
    void StopBlink()
    {
        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
            blinkCoroutine = null;
        }

        blinkVisible = true;
    }

    // Toggle cursor visibility on a fixed interval and refresh the display
    IEnumerator BlinkCursor()
    {
        while (true)
        {
            yield return new WaitForSeconds(BlinkInterval);

            blinkVisible = !blinkVisible;

            UpdateTypingDisplay();
        }
    }
}