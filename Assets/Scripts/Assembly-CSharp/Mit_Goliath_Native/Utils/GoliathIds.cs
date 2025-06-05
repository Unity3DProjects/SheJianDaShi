namespace Mit_Goliath_Native.Utils
{
	public static class GoliathIds
	{
		public sealed class GDPRStep
		{
			private readonly string step;

			public static readonly GDPRStep IMPRESSION = new GDPRStep("impression");

			public static readonly GDPRStep CLICK = new GDPRStep("click");

			private GDPRStep(string step)
			{
				this.step = step;
			}

			public override string ToString()
			{
				return step;
			}
		}

		public sealed class GDPRClickResult
		{
			private readonly string clickResult;

			public static readonly GDPRClickResult PRIVACY_POLICY = new GDPRClickResult("privacy_policy");

			public static readonly GDPRClickResult GAVE_AD_PERMISSION = new GDPRClickResult("gave_ad_permission");

			public static readonly GDPRClickResult DENIED_AD_PERMISSION = new GDPRClickResult("denied_ad_permission");

			private GDPRClickResult(string clickResult)
			{
				this.clickResult = clickResult;
			}

			public override string ToString()
			{
				return clickResult;
			}
		}

		public sealed class GDPRContext
		{
			private readonly string context;

			public static readonly GDPRContext PRIVACY_POLICY_WITH_AD_CONSENT = new GDPRContext("privacy_policy_with_ad_consent");

			public static readonly GDPRContext VIDEO_AD = new GDPRContext("video_ad");

			public static readonly GDPRContext SETTINGS_POPUP = new GDPRContext("settings_popup");

			public static readonly GDPRContext SETTINGS = new GDPRContext("settings");

			private GDPRContext(string context)
			{
				this.context = context;
			}

			public override string ToString()
			{
				return context;
			}
		}

		public sealed class ProductFamily
		{
			private readonly string name;

			public static readonly ProductFamily CHARACTER = new ProductFamily("character");

			public static readonly ProductFamily CHARACTER_UPGRADE = new ProductFamily("character_upgrade");

			private ProductFamily(string name)
			{
				this.name = name;
			}

			public override string ToString()
			{
				return name;
			}
		}

		public sealed class Action
		{
			private readonly string name;

			public static readonly Action PURCHASED = new Action("purchased");

			public static readonly Action CANCELLED = new Action("cancelled");

			public static readonly Action UPGRADE = new Action("upgrade");

			public static readonly Action ACQUISITION = new Action("acquisition");

			private Action(string name)
			{
				this.name = name;
			}

			public override string ToString()
			{
				return name;
			}
		}

		public sealed class Origin
		{
			private readonly string name;

			public static readonly Origin CHEST_ROULETTE = new Origin("chest_roulette");

			public static readonly Origin DAILY_GIFT = new Origin("chest_roulette");

			public static readonly Origin SOCIAL_SHARE = new Origin("social_share");

			public static readonly Origin SOFT_PURCHASE = new Origin("soft_purchase");

			public static readonly Origin IN_APP_PURCHASE = new Origin("in_app_purchase");

			private Origin(string name)
			{
				this.name = name;
			}

			public override string ToString()
			{
				return name;
			}
		}

		public sealed class CurrencyMovement
		{
			private readonly string name;

			public static readonly CurrencyMovement SPENT = new CurrencyMovement("spent");

			public static readonly CurrencyMovement BOUGHT = new CurrencyMovement("bought");

			public static readonly CurrencyMovement EARNED = new CurrencyMovement("earned");

			private CurrencyMovement(string name)
			{
				this.name = name;
			}

			public override string ToString()
			{
				return name;
			}
		}

		public sealed class Context
		{
			private readonly string name;

			public static readonly Context SOFT_PURCHASE = new Context("soft_purchase");

			public static readonly Context IN_APP_PURCHASE = new Context("in_app_purchase");

			public static readonly Context CHEST_ROULETTE = new Context("chest_roulette");

			public static readonly Context DAILY_GIFT = new Context("daily_gift");

			public static readonly Context GAME_PLAYED = new Context("game_played");

			public static readonly Context ACHIEVEMENT = new Context("achievement");

			public static readonly Context UPGRADE_CHARACTER = new Context("upgrade_character");

			public static readonly Context SUBSCRIPTION = new Context("subscription");

			private Context(string name)
			{
				this.name = name;
			}

			public override string ToString()
			{
				return name;
			}
		}

		public sealed class CurrencyName
		{
			private readonly string name;

			public static readonly CurrencyName COINS = new CurrencyName("coins");

			public static readonly CurrencyName GEMS = new CurrencyName("gems");

			private CurrencyName(string name)
			{
				this.name = name;
			}

			public override string ToString()
			{
				return name;
			}
		}

		public sealed class Placement
		{
			private readonly string name;

			public static readonly Placement CHARACTER_SELECTION = new Placement("character selection");

			public static readonly Placement POPUP_PACKAGE = new Placement("pop-up package");

			public static readonly Placement IN_GAME = new Placement("in-game");

			public static readonly Placement END_GAME = new Placement("end game");

			public static readonly Placement MAIN_MENU = new Placement("main menu");

			public static readonly Placement CURRENCY_SHOP = new Placement("currency shop");

			private Placement(string name)
			{
				this.name = name;
			}

			public override string ToString()
			{
				return name;
			}
		}

		public sealed class GameMode
		{
			private readonly string name;

			public static readonly GameMode VS_ROBOT = new GameMode("vs robot");

			public static readonly GameMode VS_FRIEND = new GameMode("vs friend");

			public static readonly GameMode TOURNMENT_EASY = new GameMode("tournament easy");

			public static readonly GameMode TOURNMENT_NOMAL = new GameMode("tournament normal");

			public static readonly GameMode TOURNAMENT_LEGENDARY = new GameMode("tournament legendary");

			public static readonly GameMode BIRDS_HUNT = new GameMode("birds hunt");

			public static readonly GameMode APPLE_SHOOTING = new GameMode("apple shooting");

			public static readonly GameMode ONLINE_PVP = new GameMode("online_pvp");

			public static readonly GameMode INVALID = new GameMode("invalid game mode");

			private GameMode(string name)
			{
				this.name = name;
			}

			public override string ToString()
			{
				return name;
			}

			public static GameMode GameTypeToGameMode(GameType gameType, TournamentDifficultType dificultyType)
			{
				switch (gameType)
				{
				case GameType.AppleShoot:
					return APPLE_SHOOTING;
				case GameType.BirdsHunt:
					return BIRDS_HUNT;
				case GameType.Tournament:
					switch (dificultyType)
					{
					case TournamentDifficultType.Easy:
						return TOURNMENT_EASY;
					case TournamentDifficultType.Medium:
						return TOURNMENT_NOMAL;
					case TournamentDifficultType.Hard:
						return TOURNAMENT_LEGENDARY;
					default:
						return INVALID;
					}
				case GameType.VSComputer:
					return VS_ROBOT;
				case GameType.VSHuman:
					return VS_FRIEND;
				case GameType.Multiplayer:
					return ONLINE_PVP;
				default:
					return INVALID;
				}
			}
		}

		public sealed class MatchResult
		{
			private readonly string name;

			public static readonly MatchResult WON = new MatchResult("won");

			public static readonly MatchResult LOST = new MatchResult("lost");

			private MatchResult(string name)
			{
				this.name = name;
			}

			public override string ToString()
			{
				return name;
			}
		}

		public sealed class MatchEndReason
		{
			private readonly string name;

			public static readonly MatchEndReason WON = new MatchEndReason("won");

			public static readonly MatchEndReason LOST = new MatchEndReason("lost");

			public static readonly MatchEndReason LEFT = new MatchEndReason("left");

			public static readonly MatchEndReason LEFT_MAIN_MENU = new MatchEndReason("left_main_menu");

			private MatchEndReason(string name)
			{
				this.name = name;
			}

			public override string ToString()
			{
				return name;
			}
		}

		public sealed class AdType
		{
			private readonly string name;

			public static readonly AdType VIDEO_AD = new AdType("video_ad");

			public static readonly AdType INTERTITIAL = new AdType("interstitial");

			public static readonly AdType BANNER = new AdType("banner");

			private AdType(string name)
			{
				this.name = name;
			}

			public override string ToString()
			{
				return name;
			}
		}

		public sealed class AdPhase
		{
			private readonly string name;

			public static readonly AdPhase REQUEST = new AdPhase("request");

			public static readonly AdPhase LOAD = new AdPhase("load");

			public static readonly AdPhase SHOW = new AdPhase("show");

			public static readonly AdPhase CLICKED = new AdPhase("clicked");

			private AdPhase(string name)
			{
				this.name = name;
			}

			public override string ToString()
			{
				return name;
			}
		}

		public sealed class AdReward
		{
			private readonly string name;

			public static readonly AdReward REMOVE_MISSES = new AdReward("remove misses");

			public static readonly AdReward REVIVE = new AdReward("revive");

			public static readonly AdReward EXTEND_TIME = new AdReward("extend time");

			public static readonly AdReward CHEST_ROULETTE = new AdReward("chest_roulette");

			public static readonly AdReward COINS = new AdReward("coins");

			public static readonly AdReward CHARACTER = new AdReward("try_character");

			private AdReward(string name)
			{
				this.name = name;
			}

			public override string ToString()
			{
				return name;
			}
		}

		public sealed class CustomProductIDs
		{
			private readonly string name;

			public static readonly CustomProductIDs REVIVE = new CustomProductIDs("revive");

			public static readonly CustomProductIDs EXTEND_TIME = new CustomProductIDs("extend time");

			private CustomProductIDs(string name)
			{
				this.name = name;
			}

			public override string ToString()
			{
				return name;
			}
		}

		public const string Evt_init = "init";

		public const string Evt_client_init = "client_init";

		public const string Evt_session = "session";

		public const string Evt_in_app_purchase = "in_app_purchase";

		public const string Evt_invalid_in_app_receipt = "invalid_in_app_receipt";

		public const string Evt_subscription_action = "subscription_action";

		public const string Evt_subscription_state = "subscription_state";

		public const string Evt_currency_flow = "currency_flow";

		public const string Evt_character_action = "character_action";

		public const string Evt_match = "match";

		public const string Evt_advertising = "advertising";

		public const string Evt_privacy_policy_consent = "privacy_policy_consent";

		public static string Param_timestamp = "timestamp";

		public static string Param_country = "country";

		public static string Param_latitude = "latitude";

		public static string Param_longitude = "longitude";

		public static string Param_player_wins = "player_wins";

		public static string Param_game_language = "game_language";

		public static string Param_wallet_coins = "wallet_coins";

		public static string Param_wallet_gems = "wallet_gems";

		public static string Param_nb_characters = "nb_characters";

		public static string Param_notification_id = "notification_id";

		public static string Param_libra_content = "libra_content";

		public static string Param_session_length = "session_length";

		public static string Param_product_name = "product_name";

		public static string Param_product_value = "product_value";

		public static string Param_purchase_value = "purchase_value";

		public static string Param_promotion_id = "promotion_id";

		public static string Param_context = "context";

		public static string Param_currency_name = "currency_name";

		public static string Param_currency_amount = "currency_amount";

		public static string Param_placement = "placement";

		public static string Param_wallet_coins_before = "wallet_coins_before";

		public static string Param_wallet_gems_before = "wallet_gems_before";

		public static string Param_game_mode = "game_mode";

		public static string Param_character = "character";

		public static string Param_result = "result";

		public static string Param_match_duration = "match_duration";

		public static string Param_currency_movement = "currency_movement";

		public static string Param_end_reason = "end_reason";

		public static string Param_ad_type = "ad_type";

		public static string Param_phase = "phase";

		public static string Param_provider = "provider";

		public static string Param_reward = "reward";

		public static string Param_event_age = "event_age";

		public static string Param_product_id = "product_id";

		public static string Param_product_family = "product_family";

		public static string Param_subscription_id = "subscription_id";

		public static string Param_action = "action";

		public static string Param_reference_id = "reference_id";

		public static string Param_origin = "origin";

		public static string Param_error = "receipt_error";

		public static string Param_local_currency = "local_currency";

		public static string Param_local_price = "local_price";

		public static string Param_receipt = "receipt";

		public static string Param_signed_receipt = "signed_receipt";

		public static string Param_invalid_in_app_event_id = "invalid_in_app_event_id";

		public static string Param_step = "step";

		public static string Param_click_result = "click_result";

		public static string Param_is_free_trial = "is_free_trial";

		public static string Param_in_app_purchase_event_id = "in_app_purchase_event_id";

		public static string Param_expiration_date = "expiration_date";
	}
}
