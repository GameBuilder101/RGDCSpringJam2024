using Godot;
using System;

public partial class MachineManager : Node2D
{
	public static MachineManager instance;

	public double Suspicion { get; set; } = 0.0;
	public long Moola { get; set; } = 125;
	private double Progress = 0.0;
	private double TimeBetweenTicks = 5.0;
	private Machine[] Machines;
	private MachineLocation[] Locations;
	public int FocussedLocation {get; private set;} = -1;
	private double TimePlayed = 0;
	private long HighMoola = 0;
	private int NumFines = 0;
	private bool Lost = false;

	[Export]
	private Node LocationHolder;
	[Export]
	private FadeOverlay Overlay;
	[Export]
	private int fineAmount;

	[Export]
	private EventPlayableAudio _focusAudio;
	public bool GotJackpot { get; set; }
	[Export]
	private EventPlayableAudio _jackpotAudio;
	public bool GotHouseWin { get; set; }
	[Export]
	private EventPlayableAudio _noJackpotAudio;
	[Export]
	private EventPlayableAudio _finedAudio;

	public int TotalMachineRollCount { get; private set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		instance = this;
		int num = LocationHolder.GetChildCount();
		Locations = new MachineLocation[num];
		int index = 0;
		MachineLocation loc;
		foreach(Node l in LocationHolder.GetChildren()) {
			loc = (MachineLocation) l;
			Locations[index] = loc;
			loc.setIndex(index);
			index += 1;
		}
		Machines = new Machine[num];
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		TimePlayed += delta;
		Progress += delta;
		if (Progress >= TimeBetweenTicks) {
			Progress -= TimeBetweenTicks;
			foreach (Machine m in Machines) {
				if (m == null) {
					continue;
				}
				m.Tick();
			}
		}
		if (Suspicion >= 1) {
			int fine = 0;
			NumFines += 1;
			if(NumFines == 1){
				fine = 100;
			}
			else if(NumFines == 2){
				fine = 500;
			}
			else{
				fine = (NumFines-1) * fineAmount;
			}
			
			Moola -= fine;
			Suspicion = 0;
			TextParticleManager.instance.createFineMessage(
				NumFines, fine
			);
		}
		if (Moola < 0 && !Lost) {
			Overlay.fadeOut();
			GameOver.SetScoreValues(TimePlayed, HighMoola, NumFines);
			Lost = true;
		}
		if (Moola > HighMoola) {
			HighMoola = Moola;
		}

		if (GotJackpot)
		{
			_jackpotAudio.Play();
			GotJackpot = false;
		}

		if (GotHouseWin)
		{
			_noJackpotAudio.Play();
			GotHouseWin = false;
		}
	}
	
	public void placeMachine(Machine m) {
		placeMachine(FocussedLocation, m);
	}
	
	public void placeMachine(int index, Machine m) {
		m.Position = Locations[index].Position + new Vector2(0, -8);
		Machines[index] = m;
		AddChild(m);

		TotalMachineRollCount += m.NumRolls;
	}
	
	public void removeMachine(int index) {
		TotalMachineRollCount -= Machines[index].NumRolls;

		Machines[index].QueueFree();
		Machines[index] = null;
	}
	
	public Machine getMachine(int index) {
		return Machines[index];
	}
	
	public Machine getMachine() {
		return getMachine(FocussedLocation);
	}
	
	// -1 to unfocus
	public void focus(int index) {
		if (FocussedLocation != -1) {
			Locations[FocussedLocation].Focus = false;
		}
		FocussedLocation = index;
		if (FocussedLocation != -1) {
			Locations[FocussedLocation].Focus = true;
		}
		_focusAudio.Play();
	}
}
