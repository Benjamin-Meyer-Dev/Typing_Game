// =============================================================================
// WordLists.cs
// Static word and phrase lists organised by level and keyboard row focus.
// =============================================================================

public static class WordLists
{
    // -------------------------------------------------------------------------
    // Middle Row  (a s d f g h j k l)
    // -------------------------------------------------------------------------

    // Individual middle-row keys
    public static readonly string[] Level1 =
    {
        "a", "s", "d", "f", "g", "h", "j", "k", "l"
    };

    // Short words using only middle-row letters
    public static readonly string[] Level2 =
    {
        "ask", "lad", "sad", "add", "fall",
        "salad", "flask", "lass", "dads", "fads",
        "gal", "lash", "half", "hall", "glad",
        "flag", "dash", "all", "has", "shall"
    };

    // Two or three word phrases using only middle-row letters
    public static readonly string[] Level3 =
    {
        "sad lad",
        "glad dad",
        "all fall",
        "add a dash",
        "half a flask",
        "a glad gal",
        "ask a lad",
        "all halls",
        "has a flag",
        "a sad lass",
        "shall fall",
        "a half flask"
    };

    // -------------------------------------------------------------------------
    // Top Row  (q w e r t y u i o p)
    // -------------------------------------------------------------------------

    // Individual top-row keys
    public static readonly string[] Level4 =
    {
        "q", "w", "e", "r", "t", "y", "u", "i", "o", "p"
    };

    // Short words using only top-row letters
    public static readonly string[] Level5 =
    {
        "out", "our", "you", "rut", "try",
        "toy", "top", "tip", "tie", "pie",
        "pit", "pot", "put", "wit", "woe",
        "wore", "your", "trip", "pout", "pour"
    };

    // Two or three word phrases using only top-row letters
    public static readonly string[] Level6 =
    {
        "try it out",
        "your toy",
        "pour it out",
        "our top tip",
        "tie it up",
        "a top trip",
        "you wore it",
        "put it out",
        "try your best",
        "tip or pot",
        "pour your tea",
        "a toy top"
    };

    // -------------------------------------------------------------------------
    // Middle + Top Rows
    // -------------------------------------------------------------------------

    // Short words mixing middle and top rows
    public static readonly string[] Level7 =
    {
        "fork", "joke", "lake", "rake", "rate",
        "gate", "take", "late", "rope", "role",
        "rule", "rose", "pure", "yoke", "poke",
        "tire", "flop", "toad", "load", "lore"
    };

    // Two or three word phrases mixing middle and top rows
    public static readonly string[] Level8 =
    {
        "take the fork",
        "at the gate",
        "by the lake",
        "a late joke",
        "the toad sat",
        "a pure rose",
        "load the rope",
        "poke the toad",
        "a flat tire",
        "rake the leaf",
        "the role play",
        "a yoke and rope"
    };

    // Short sentences using middle and top rows with punctuation
    public static readonly string[] Level9 =
    {
        "Take the fork.",
        "The toad sat still.",
        "A pure rose fell.",
        "Late to the gate!",
        "The lake was flat.",
        "Load the rope up.",
        "Did the toad leap?",
        "Rake the old leaf.",
        "She told a joke.",
        "The role was great."
    };

    // -------------------------------------------------------------------------
    // Bottom Row  (z x c v b n m)
    // -------------------------------------------------------------------------

    // Individual bottom-row keys
    public static readonly string[] Level10 =
    {
        "z", "x", "c", "v", "b", "n", "m"
    };

    // Short words using middle and bottom rows
    public static readonly string[] Level11 =
    {
        "can", "man", "ban", "van", "cab",
        "nab", "clan", "clam", "scan", "slam",
        "back", "lack", "black", "blank", "band",
        "clank", "sand", "land", "hand", "mask"
    };

    // Two or three word phrases using middle and bottom rows
    public static readonly string[] Level12 =
    {
        "black sand",
        "a blank canvas",
        "back the van",
        "scan the land",
        "the man can",
        "a jazz band",
        "slam dunk",
        "hand in hand",
        "a black cab",
        "clam and crab",
        "a blank mask",
        "clan of man"
    };

    // Short sentences using middle and bottom rows with punctuation
    public static readonly string[] Level13 =
    {
        "The man had a black van.",
        "Can she scan the land?",
        "The band played all night.",
        "He held sand in his hand.",
        "A black cab came back.",
        "Slam the door shut!",
        "The clan camped on the land.",
        "She clammed up and left.",
        "Back the van in slowly.",
        "The mask was blank and black."
    };

    // -------------------------------------------------------------------------
    // All Rows
    // -------------------------------------------------------------------------

    // Short words using all rows
    public static readonly string[] Level14 =
    {
        "big", "box", "mix", "fix", "fox",
        "zip", "buzz", "wave", "cave", "gave",
        "save", "zoom", "fun", "sun", "run",
        "jump", "vine", "zone", "fuzz", "wink"
    };

    // Medium words using all rows
    public static readonly string[] Level15 =
    {
        "brave", "close", "drive", "flame", "phone",
        "place", "plant", "share", "shine", "smile",
        "snack", "sweet", "shelf", "swing", "storm",
        "clock", "brush", "fresh", "blend", "crisp"
    };

    // Two or three word phrases using all rows
    public static readonly string[] Level16 =
    {
        "big brave fox",
        "save the cave",
        "jump and run",
        "fresh and crisp",
        "a bright smile",
        "fix the clock",
        "zoom and zip",
        "sweet snack time",
        "brush and blend",
        "fun in the sun",
        "drive and wave",
        "share the shelf"
    };

    // Short sentences using all rows
    public static readonly string[] Level17 =
    {
        "The fox ran fast.",
        "Jump over the box!",
        "Save your snack for later.",
        "She gave a big smile.",
        "Fix the clock on the shelf.",
        "The cave was dark and cold.",
        "Run and zoom to the finish!",
        "He brushed his teeth twice.",
        "The storm made the waves big.",
        "Blend it fresh every morning."
    };

    // Proper nouns — capitals practice
    public static readonly string[] Level18 =
    {
        "Alice", "Bobby", "Cindy", "Danny", "Emily",
        "Frankie", "Grace", "Henry", "Isla", "Jake",
        "Katie", "Liam", "Mia", "Noah", "Olivia",
        "Peter", "Quinn", "Ruby", "Sam", "Tina"
    };

    // Short sentences with proper nouns
    public static readonly string[] Level19 =
    {
        "Alice likes cats.",
        "Bobby can run fast.",
        "Cindy loves to read.",
        "Danny has a red bike.",
        "Emily feeds the ducks.",
        "Frankie bakes muffins.",
        "Grace draws and paints.",
        "Henry found a snail.",
        "Isla climbs the big tree.",
        "Jake plays with his dog.",
        "Katie sings every day.",
        "Liam built a sandcastle.",
        "Mia helps in the garden.",
        "Noah found a cool rock.",
        "Olivia wrote a short story."
    };

    // Longer sentences with proper nouns and all punctuation
    public static readonly string[] Level20 =
    {
        "Tom and Lily went to the park after school.",
        "The big brown dog chased the ball all the way down the hill.",
        "Maya loves strawberries, blueberries, and cream on her oats.",
        "Jack jumped over the puddle but still got his shoes wet.",
        "Zoe and Ben flew their kite high above the trees.",
        "The whole class went on a trip to the science museum.",
        "Ruby found a tiny frog hiding by the edge of the pond.",
        "Oscar read his favourite book twice before going to sleep.",
        "The twins built a cosy fort out of blankets and pillows.",
        "Finn and Clara raced all the way down the big hill."
    };

    // Two sentence stories
    public static readonly string[] Level21 =
    {
        "Nina baked cookies for her whole class. Everyone got two each!",
        "The little owl sat quietly in the oak tree. It blinked its big eyes slowly.",
        "Leo drew a rocket ship on the sidewalk with chalk. He made the flames bright orange.",
        "Daisy skipped all the way to the corner store. She bought a cold drink and a snack.",
        "Every morning Max feeds his three goldfish. They swim up to the top when they see him.",
        "The rain fell hard all afternoon. We stayed inside and played board games.",
        "She sang her favourite song at the top of her lungs. Everyone in the house started clapping.",
        "First he tied his shoes, then he grabbed his bag. He ran to catch the bus just in time.",
        "The snow fell all night and covered the garden. In the morning everything was bright white.",
        "We stayed up late to watch the stars. Dad showed us how to find the Big Dipper."
    };

    // Longer two sentence stories with all punctuation
    public static readonly string[] Level22 =
    {
        "The old cat found a sunny spot on the windowsill. She curled up and slept there all afternoon.",
        "Jake packed his bag the night before the trip. He did not want to forget his swimming goggles.",
        "The puppy chewed through its third toy this week. Mum said it was time to buy something tougher.",
        "Grace practised her lines every day before the show. On the big night, she remembered every single word.",
        "The thunderstorm knocked out the power for two hours. We ate dinner by candlelight and told spooky stories.",
        "Noah planted sunflower seeds in the garden in spring. By summer they were taller than he was.",
        "Liam forgot his lunch on the kitchen bench that morning. His friend shared a sandwich with him at noon.",
        "The baby penguin waddled across the ice towards its mum. She had been out fishing and was finally back.",
        "Ruby tried the climbing wall for the very first time. She made it halfway up before her arms gave out.",
        "The little robot beeped twice and started to spin. The kids laughed and chased it around the room."
    };

    // Multi-clause sentences with varied punctuation
    public static readonly string[] Level23 =
    {
        "Tom read the map, Clara packed the bag, and they set off just after breakfast.",
        "The wind picked up, the trees began to sway, and the rain started pouring down.",
        "Did you finish the puzzle? If not, keep going! You are so close to the end.",
        "She mixed the batter, poured it into the tin, and slid it into the warm oven.",
        "He woke up early, got dressed quickly, ate his toast, and ran out the front door.",
        "The bus was late again, but we still made it to school just before the bell rang.",
        "What a brilliant game that was! Both teams played hard, and the crowd went wild.",
        "She drew a picture, coloured it carefully, and gave it to her dad as a surprise.",
        "The kite went up, dipped to the left, spun in a circle, and then soared up high.",
        "Plan it out first, give it your best shot, and ask for help if you need it."
    };

    // Longer polished sentences using all keys and all punctuation
    public static readonly string[] Level24 =
    {
        "The quick brown fox jumped over the sleeping dog and kept on running.",
        "Zoe and Quinn mixed up a fizzy drink and dared each other to try it first.",
        "Every morning the five friends met at the corner and walked to school together.",
        "Jack and Max trained every single day that summer, and it really paid off.",
        "Olivia, Sam, and Grace went camping for the weekend and told stories by the fire.",
        "The bright red kite climbed higher and higher until it was just a dot in the sky.",
        "We grabbed our bags, sprinted to the bus stop, and made it just as the doors closed.",
        "First we swam in the lake, then we dried off in the sun, then we had hot chocolate.",
        "Ruby loves to paint, Liam loves to build things, and Noah loves to explore outside.",
        "Every single day is a brand new chance to try something and get a little bit better."
    };
}