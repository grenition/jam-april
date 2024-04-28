public static class Difficulty
{
    private static int _difficultyNumber;

    public static void SetDifficulty(int value)
    {
        _difficultyNumber = value;
    }

    public static float GetPlayerHealth()
    {
        if (_difficultyNumber == 0)
            return 250;
        else if (_difficultyNumber == 1)
            return 100;
        else
            return 60;
    }
}
