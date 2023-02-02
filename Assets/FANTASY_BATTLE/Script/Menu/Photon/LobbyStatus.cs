using UnityEngine;

namespace FantasyBattle
{
    public class LobbyStatus
    {
        //ROOM PROPERTIES
        public const string CURRENT_COUNT_ENEMIES = "CURRENT_COUNT_ENEMIES";
        public const string EMPTY = "EMPTY";
        //ENEMY DATA
        public const float ENEMY_SPAWN_TIME = 5.0f;
        public const float ENEMY_MAX_SPAWN_TIME = 10.0f;
        public const string ENEMY_TAG = "Enemy";
        //PLAYER DATA
        public const string PLAYER_TAG = "Player";
        public const float PLAYER_RESPAWN_TIME = 4.0f;
        public const int PLAYER_MAX_LIVES = 3;
        public const string PLAYER_LIVES = "PlayerLives";
        public const string PLAYER_READY = "IsPlayerReady";
        public const string PLAYER_LOADED_LEVEL = "PlayerLoadedLevel";
        //Player CustomProperties
        public const string USER_NAME = "User Name";
        public const string GROUP_COVEN = "Group Coven";
        public const string RED_COVEN = "Red coven";
        public const string BLUE_COVEN = "Blue coven";
        //Room CustomProperties
        public const string RED_COVEN_COUNT_PLAYERS = "RED_COVEN_COUNT_PLAYERS";
        public const string BLUE_COVEN_COUNT_PLAYERS = "BLUE_COVEN_COUNT_PLAYERS";
        //CharacterProperties
        public const string CHARACTER_ID = "CHARACTER_ID";
        public const string NAME_CLASS = "NAME_CLASS";
        public const string CHARACTER_LEVEL = "CHARACTER_LEVEL";
        public const string CHARACTER_HP = "CHARACTER_HP";
        public const string CURRENT_HP = "CURRENT_HP";
        public const string CHARACTER_MP = "CHARACTER_MP";
        public const string CURRENT_MP = "CURRENT_MP";
        public const string CHARACTER_DAMAGE = "CHARACTER_DAMAGE";
        public const string CHARACTER_EXP = "CHARACTER_EXP";
        //CharacterSetting in game
        public const string CHARACTER_KILLS = "CHARACTER_KILLS";
        public const string CHARACTER_COUNTDAMAGE = "CHARACTER_COUNTDAMAGE";

         public static Color GetColor(int colorChoice)
        {
            switch (colorChoice)
            {
                case 0: return Color.red;
                case 1: return Color.green;
                case 2: return Color.blue;
                case 3: return Color.yellow;
                case 4: return Color.cyan;
                case 5: return Color.grey;
                case 6: return Color.magenta;
                case 7: return Color.white;
            }

            return Color.black;
        }
    }
}
