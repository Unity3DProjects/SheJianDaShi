using System.Runtime.InteropServices;

[StructLayout((LayoutKind)0, Size = 1)]
public struct GamePrefs
{
	public const string COINS_KEY = "Coins";

	public const string GEMS_KEY = "Gems";

	public const string BIRDS_KILLED = "birds_killed";

	public const string HIGHSCORE_BIRDS_KILLED = "highscore_birds_killed";

	public const string FRUITS_SMASHED = "fruits_smashed";

	public const string HIGHSCORE_FRUITS_SMASHED = "highscore_fruits_smashed";

	public const string TOTAL_DAMAGE_APPLIED = "total_damage_applied";

	public const string WINS_COUNT = "wins_count";

	public const string LOSES_COUNT = "loses_count";

	public const string MATCHES_COUNT_VS_HUMAN = "matches_count_vs_human";

	public const string MATCHES_COUNT_MULTIPLAYER = "matches_count_multiplayer";

	public const string MATCHES_WINS_MULTIPLAYER = "matches_wins_multiplayer";

	public const string MATCHES_COUNT = "matches_count";

	public const string LAST_MATCHES_IN_DAY = "last_matches_in_day";

	public const string COUNT_MATCHES_IN_DAY = "count_matches_in_day";

	public const string TOTAL_GAME_TIME = "total_game_time";

	public const string RATE_KEY = "rate_game";

	public const string FORCED_RATE_KEY = "forced_rate_game";

	public const string OPENED_CHESTS = "opened_chets";

	public const string FREE_CHESTS_OPENED = "free_chests_opened";

	public const string UNLOCKED_GAME_TYPES = "unlocked_game_types";

	public const string MENU_SCREENS_COUNT = "menu_screens_count";

	public const string OPENED_GIFT_NUMBERS = "opened_gift_numbers";

	public const string LAST_RATE = "last_rate";

	public const string LAST_NO_ADS = "last_no_ads";

	public const string LAST_FOOLISH_SUBSCRIBE = "last_foolish_subscribe";

	public const string LAST_WIN_DATE = "last_win_date";

	public const string LAST_FIRST_DAILY_WIN_DATE = "last_first_daily_win_date";

	public const string FIRST_LAUNCH_TIME = "first_launch_time";

	public const string LAST_MISTERY_BOX = "last_mistery_box";

	public const string LAST_CHARACTER_KEY = "last_character";

	public const string LAST_CHARACTER_NAME_TYPE__KEY = "last_character_name_type";

	public const string COUNT_LAST_CHARACTER_KEY = "count_last_character";

	public const string DOUBLE_COINS_KEY = "double_coins";

	public const string LAST_DEVICE_TYPE_SENDING_DATE = "last_device_type_date";

	public const string VIDEOS_WATCHED = "videos_watched";

	public const string TOURNAMENT_EASY_WIN = "tournament_easy_win";

	public const string TOURNAMENT_MEDIUM_WIN = "tournament_medium_win";

	public const string TOURNAMENT_HARD_WIN = "tournament_hard_win";

	public const string LAST_PROPOSAL_WITH_TIMER = "last_proposal_with_timer";

	public const string LAST_PROPOSAL_WITH_TIMER_TYPE = "last_proposal_with_timer_type";

	public const string LAST_PROPOSAL_WITH_TIMER_START = "last_proposal_with_timer_start";

	public const string LAST_FREE_CHEST_TIMER_START = "last_free_chest_timer_start";

	public const string LAST_FREE_CHEST_ACCELERATOR = "last_free_chest_accelerator";

	public const string LAST_TRY_CHARACTER = "last_try_character";

	public const string LAST_PACK = "last_pack";

	public const string LAST_UPGRADE = "last_upgrade";

	public const string LAST_CHAMPION_PACK = "last_champion_pack";

	public const string LAST_VIP_PACK = "last_vip_pack";

	public const string LAST_DOUBLE_COINS = "last_double_coins";

	public const string LAST_FREE_COINS = "last_free_coins";

	public const string CHARACTERS_BOUGHT_ON_BUCKS = "characters_bought_on_bucks";

	public const string GIFTS_OPENED = "gifts_opened";

	public const string SPENT_BUCKS = "spent_bucks";

	public const string LAST_TRY_CHARACTER_ID = "last_try_character_id";

	public const string LAST_BUT_ONE_TRY_CHARACTER_ID = "last_but_one_try_character_id";

	public const string LAST_TRY_CHARACTER_MATCHES = "last_try_character_matches";

	public const string LAST_TRY_CHARACTER_GEN_MATCHES = "last_try_character_gen_matches";

	public const string DEFEATED_CHARACTERS = "defeated_characters";

	public const string DEFEATED_CHARACTERS_64 = "defeated_characters_64";

	public const string DEFEATED_CHARACTERS_LIST = "defeated_characters_list";

	public const string FACEBOOK_CLICKED = "facebook_clicked";

	public const string INSTAGRAM_CLICKED = "instagram_clicked";

	public const string TWITTER_CLICKED = "twitter_clicked";

	public const string FACEBOOK_MERCIFY_CLICKED = "facebook_mercify_clicked";

	public const string INSTAGRAM_MERCIFY_CLICKED = "instagram_mercify_clicked";

	public const string TWITTER_MERCIFY_CLICKED = "twitter_mercify_clicked";

	public const string VK_MERCIFY_CLICKED = "vk_mercify_clicked";

	public const string LAST_SOCIAL_POPUP_MATCHES = "last_social_popup_matches";

	public const string LAST_SOCIAL_POPUP_TYPE = "last_social_popup_type";

	public const string AD_PRIORITY_WEIGHTS = "ad_priority_weights";

	public const string AD_PRIORITY_LIST = "ad_priority_list";

	public const string INVITES_COUNT = "invites_count";

	public const string DIFFERENT_INVITES_COUNT = "different_invites_count";

	public const string NECESARY_INVITED_CHARACTERS = "necesary_invited_characters";

	public const string ICLOUD_STORE = "icloud_store";

	public const string LANGUAGE_TYPE = "language_type";

	public const string FORCED_LOCATIONS_KEY = "forced_locations_key";

	public const string CHARACTER_ADS_COUNT_KEY = "character_ads_count";

	public const string CHARACTER_ADS_COUNT_BY_DAY = "character_ads_count_by_day";

	public const string CHARACTER_ADS_TIMER_ENDED = "character_timer_started";

	public const string CHARACTER_ADS_NOTIFICATIONS = "character_timer_notifications";

	public const string FREE_GEMS_RECEIVED = "free_gems_received";

	public const string FREE_GEMS_INVITES_COUNT = "free_gems_invites_count";

	public const string FREE_GEMS_ADS_COUNT_KEY = "free_gems_ads_count";

	public const string FREE_GEMS_ADS_COUNT_BY_DAY = "free_gems_ads_count_by_day";

	public const string FREE_GEMS_ADS_TIMER_ENDED = "free_gems_timer_ended";

	public const string SUBSCRIPTION_REWARD_DATE = "subscribe_reward_date";

	public const string GEMS_BOUGHT = "gems_bought";

	public const string LAST_ACTIVE_SUBSCRIPTION = "last_active_subscription";

	public const string SUBSCRIPTION_WAS_ACTIVATED = "subscription_was_activated";

	public const string LAST_REAL_UTC_DATE = "last_real_utc_date";

	public const string LAST_SUPER_GIFT_DATE = "last_super_gift_date";

	public const string SUPER_GIFT_NUMBER = "super_gift_number";

	public const string MISTERY_BOX_NUMBER = "mistery_box_number";

	public const string ROULETTE_PLAYED_CHARACTERS = "roulette_played_characters";

	public const string LAST_ROULETTE_PLAYED_DAY = "last_roulette_played_day";

	public const string PARTY_MASTERS_BUTTON_CLICKED = "party_masters_button_clicked";

	public const string LAST_MATCH_SHOWED_PARTY_MASTERS_ANNONSER = "last_match_showed_party_masters_announcer";

	public const string LAST_PROMO_FETCHER = "last_promo_fetcher";

	public const string RATE_PENDING = "rate_pending";
}
