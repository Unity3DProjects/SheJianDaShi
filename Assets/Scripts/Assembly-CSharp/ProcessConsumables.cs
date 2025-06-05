using System.Collections.Generic;
using Prime31;

public class ProcessConsumables
{
	private Queue<Consumable> consumables;

	private Consumable currentConsumable;

	private static bool isConsuming;

	private const int MAX_CONSUME_ATTEMPTS = 3;

	public ProcessConsumables()
	{
		Initialize();
	}

	public void Initialize()
	{
		consumables = new Queue<Consumable>();
	}

	public void OnEnable()
	{
		GoogleIABManager.consumePurchaseSucceededEvent += ConsumeSucceededCallback;
		GoogleIABManager.consumePurchaseFailedEvent += ConsumeFailedCallback;
	}

	public void OnDisable()
	{
		GoogleIABManager.consumePurchaseSucceededEvent -= ConsumeSucceededCallback;
		GoogleIABManager.consumePurchaseFailedEvent -= ConsumeFailedCallback;
	}

	public void CreateConsumable(string productId, Receipt receipt)
	{
		Consumable item = new Consumable(productId, receipt, 3);
		consumables.Enqueue(item);
	}

	private void ConsumeSucceededCallback(GooglePurchase purchase)
	{
		isConsuming = false;
		MCInnAppsmanager.ConsumeResultForwarder.OnConsumeSucceeeded(purchase, currentConsumable);
		MiniclipUtils.DismissReceiptValidatingPopup();
	}

	private void ConsumeFailedCallback(string error)
	{
		string productId = error.Split(':')[0];
		if (currentConsumable.ConsumeAttempts > 0)
		{
			Consume(currentConsumable);
		}
		else
		{
			isConsuming = false;
			MCInnAppsmanager.ConsumeResultForwarder.OnConsumeFailed(productId, error);
		}
		MiniclipUtils.DismissReceiptValidatingPopup();
	}

	private void Consume(Consumable consumable)
	{
		GoogleIAB.consumeProduct(consumable.ProductId);
		consumable.ConsumeAttempts--;
	}

	public void Update()
	{
		if (!isConsuming && consumables.Count != 0)
		{
			currentConsumable = consumables.Dequeue();
			isConsuming = true;
			Consume(currentConsumable);
		}
	}
}
