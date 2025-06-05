public class Managers : SingletonMonoBehaviour<Managers>
{
	private GameManager cachedGameManager;

	private LevelsManager cachedLevelsManager;

	private EffectsManager cachedEffectsManager;

	private ProposalManager cachedProposalManager;

	private StatisticsManager cachedStatisticsManager;

	private RouletteManager cachedRouletteManager;

	private TournamentManager cachedTournamentManager;

	private TutorialManager cachedTutorialManager;

	private MicroachievementsManager cachedMicroachievementsManager;

	private GUIScreenManager cachedGUIScreenManager;

	private GUIPopupManager cachedGUIPopupManager;

	private ShopManager cachedShopManager;

	private tk2dUIManager cachedTK2DUIManager;

	private Scheduler cachedScheduler;

	private LocalisationManager cachedLocalisationManager;

	private NotifyManager cachedNotifyManager;

	private AdvertisingManager cachedAdvertisingManager;

	private GUIOrientationManger cachedGUIOrientationManager;

	private MultiplayerManager cachedMultiplayerManager;

	private GiftManager cachedGiftManager;

	private AudioManager cachedAudioManager;

	private GameCenterManager cachedGameCenterManager;

	private InviteManager cachedInviteManager;

	private SubscribeManager cachedSubscribeManager;

	private ResourceManager cachedResourceManager;

	private LocationsManager cachedLocationsManager;

	private SubscriptionManager cachedSubscriptionManager;

	private RemoteManager cachedRemoteManager;

	public GameManager CachedGameManager
	{
		get
		{
			if (cachedGameManager == null)
			{
				cachedGameManager = SingletonMonoBehaviour<GameManager>.Instance;
			}
			return cachedGameManager;
		}
	}

	public LevelsManager CachedLevelsManager
	{
		get
		{
			if (cachedLevelsManager == null)
			{
				cachedLevelsManager = SingletonMonoBehaviour<LevelsManager>.Instance;
			}
			return cachedLevelsManager;
		}
	}

	public EffectsManager CachedEffectsManager
	{
		get
		{
			if (cachedEffectsManager == null)
			{
				cachedEffectsManager = SingletonMonoBehaviour<EffectsManager>.Instance;
			}
			return cachedEffectsManager;
		}
	}

	public ProposalManager CachedProposalManager
	{
		get
		{
			if (cachedProposalManager == null)
			{
				cachedProposalManager = SingletonMonoBehaviour<ProposalManager>.Instance;
			}
			return cachedProposalManager;
		}
	}

	public StatisticsManager CachedStatisticsManager
	{
		get
		{
			if (cachedStatisticsManager == null)
			{
				cachedStatisticsManager = SingletonMonoBehaviour<StatisticsManager>.Instance;
			}
			return cachedStatisticsManager;
		}
	}

	public RouletteManager CachedRouletteManager
	{
		get
		{
			if (cachedRouletteManager == null)
			{
				cachedRouletteManager = SingletonMonoBehaviour<RouletteManager>.Instance;
			}
			return cachedRouletteManager;
		}
	}

	public TournamentManager CachedTournamentManager
	{
		get
		{
			if (cachedTournamentManager == null)
			{
				cachedTournamentManager = SingletonMonoBehaviour<TournamentManager>.Instance;
			}
			return cachedTournamentManager;
		}
	}

	public TutorialManager CachedTutorialManager
	{
		get
		{
			if (cachedTutorialManager == null)
			{
				cachedTutorialManager = SingletonMonoBehaviour<TutorialManager>.Instance;
			}
			return cachedTutorialManager;
		}
	}

	public MicroachievementsManager CachedMicroachievementsManager
	{
		get
		{
			if (cachedMicroachievementsManager == null)
			{
				cachedMicroachievementsManager = SingletonMonoBehaviour<MicroachievementsManager>.Instance;
			}
			return cachedMicroachievementsManager;
		}
	}

	public GUIScreenManager CachedGUIScreenManager
	{
		get
		{
			if (cachedGUIScreenManager == null)
			{
				cachedGUIScreenManager = SingletonMonoBehaviour<GUIScreenManager>.Instance;
			}
			return cachedGUIScreenManager;
		}
	}

	public GUIPopupManager CachedGUIPopupManager
	{
		get
		{
			if (cachedGUIPopupManager == null)
			{
				cachedGUIPopupManager = SingletonMonoBehaviour<GUIPopupManager>.Instance;
			}
			return cachedGUIPopupManager;
		}
	}

	public ShopManager CachedShopManager
	{
		get
		{
			if (cachedShopManager == null)
			{
				cachedShopManager = SingletonMonoBehaviour<ShopManager>.Instance;
			}
			return cachedShopManager;
		}
	}

	public tk2dUIManager CachedTK2DUIManager
	{
		get
		{
			if (cachedTK2DUIManager == null)
			{
				cachedTK2DUIManager = tk2dUIManager.Instance;
			}
			return cachedTK2DUIManager;
		}
	}

	public Scheduler CachedScheduler
	{
		get
		{
			if (cachedScheduler == null)
			{
				cachedScheduler = SingletonMonoBehaviour<Scheduler>.Instance;
			}
			return cachedScheduler;
		}
	}

	public LocalisationManager CachedLocalisationManager
	{
		get
		{
			if (cachedLocalisationManager == null)
			{
				cachedLocalisationManager = SingletonMonoBehaviour<LocalisationManager>.Instance;
			}
			return cachedLocalisationManager;
		}
	}

	public NotifyManager CachedNotifyManager
	{
		get
		{
			if (cachedNotifyManager == null)
			{
				cachedNotifyManager = SingletonMonoBehaviour<NotifyManager>.Instance;
			}
			return cachedNotifyManager;
		}
	}

	public AdvertisingManager CachedAdvertisingManager
	{
		get
		{
			if (cachedAdvertisingManager == null)
			{
				cachedAdvertisingManager = SingletonMonoBehaviour<AdvertisingManager>.Instance;
			}
			return cachedAdvertisingManager;
		}
	}

	public GUIOrientationManger CachedGUIOrientationManager
	{
		get
		{
			if (cachedGUIOrientationManager == null)
			{
				cachedGUIOrientationManager = SingletonMonoBehaviour<GUIOrientationManger>.Instance;
			}
			return cachedGUIOrientationManager;
		}
	}

	public MultiplayerManager CachedMultiplayerManager
	{
		get
		{
			if (cachedMultiplayerManager == null)
			{
				cachedMultiplayerManager = SingletonMonoBehaviour<MultiplayerManager>.Instance;
			}
			return cachedMultiplayerManager;
		}
	}

	public GiftManager CachedGiftManager
	{
		get
		{
			if (cachedGiftManager == null)
			{
				cachedGiftManager = SingletonMonoBehaviour<GiftManager>.Instance;
			}
			return cachedGiftManager;
		}
	}

	public AudioManager CachedAudioManager
	{
		get
		{
			if (cachedAudioManager == null)
			{
				cachedAudioManager = SingletonMonoBehaviour<AudioManager>.Instance;
			}
			return cachedAudioManager;
		}
	}

	public GameCenterManager CachedGameCenterManager
	{
		get
		{
			if (cachedGameCenterManager == null)
			{
				cachedGameCenterManager = SingletonMonoBehaviour<GameCenterManager>.Instance;
			}
			return cachedGameCenterManager;
		}
	}

	public InviteManager CachedInviteManager
	{
		get
		{
			if (cachedInviteManager == null)
			{
				cachedInviteManager = SingletonMonoBehaviour<InviteManager>.Instance;
			}
			return cachedInviteManager;
		}
	}

	public SubscribeManager CachedSubscribeManager
	{
		get
		{
			if (cachedSubscribeManager == null)
			{
				cachedSubscribeManager = SingletonMonoBehaviour<SubscribeManager>.Instance;
			}
			return cachedSubscribeManager;
		}
	}

	public ResourceManager CachedResourceManager
	{
		get
		{
			if (cachedResourceManager == null)
			{
				cachedResourceManager = SingletonMonoBehaviour<ResourceManager>.Instance;
			}
			return cachedResourceManager;
		}
	}

	public LocationsManager CachedLocationsManager
	{
		get
		{
			if (cachedLocationsManager == null)
			{
				cachedLocationsManager = SingletonMonoBehaviour<LocationsManager>.Instance;
			}
			return cachedLocationsManager;
		}
	}

	public SubscriptionManager CachedSubscriptionManager
	{
		get
		{
			if (cachedSubscriptionManager == null)
			{
				cachedSubscriptionManager = SingletonMonoBehaviour<SubscriptionManager>.Instance;
			}
			return cachedSubscriptionManager;
		}
	}

	public RemoteManager CachedRemoteManager
	{
		get
		{
			if (cachedRemoteManager == null)
			{
				cachedRemoteManager = SingletonMonoBehaviour<RemoteManager>.Instance;
			}
			return cachedRemoteManager;
		}
	}
}
