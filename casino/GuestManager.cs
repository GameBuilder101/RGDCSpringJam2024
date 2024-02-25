using Godot;
using System;
using System.Collections.Generic;

public partial class GuestManager : Node
{
	[Export]
	private Node2D _guestParent;
	[Export]
	private GuestMovementNode _startingNode;
	[Export]
	private PackedScene _templateGuest;
    [Export]
    private PackedScene _templateInspector;

    private List<Guest> _currentGuests;
	private Guest _inspector;

	[Export]
	private int _maxGuests = 12;
	public int TargetGuestAmount
	{
		get
		{
			int count = MachineManager.instance.TotalMachineRollCount;
			if (count > _maxGuests)
				return _maxGuests;
			return count;
		}
	}
	private double _guestTimer;
	[Export]
	private double _timeForNewGuest = 14.0;

	[Export]
	private double _inspectorThreshold = 0.8;
	public bool ShouldInspectorBeVisible { get { return MachineManager.instance.Suspicion >= _inspectorThreshold; } }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_currentGuests = new List<Guest>();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (ShouldInspectorBeVisible && _inspector == null)
			SpawnInspector();
		else if (!ShouldInspectorBeVisible && _inspector != null)
			RemoveInspector();

		if (_currentGuests.Count == TargetGuestAmount)
		{
			_guestTimer = 0.0;
			return;
		}

        _guestTimer += delta;
        if (_guestTimer >= _timeForNewGuest)
        {
            _guestTimer = 0.0;
            if (_currentGuests.Count < TargetGuestAmount)
                AddGuest();
            else if (_currentGuests.Count > TargetGuestAmount)
                RemoveGuest();
        }
    }

	public void AddGuest()
	{
		Guest newGuest = (Guest)_templateGuest.Instantiate();
        newGuest.Destination = _startingNode;
        _guestParent.AddChild(newGuest);
        _currentGuests.Add(newGuest);
	}

	public void RemoveGuest()
	{
		_currentGuests[_currentGuests.Count - 1].Despawn();
		_currentGuests.RemoveAt(_currentGuests.Count - 1);
	}

	public void SpawnInspector()
	{
		_inspector = (Guest)_templateInspector.Instantiate();
		_inspector.Destination = _startingNode;
		_guestParent.AddChild(_inspector);
    }

	public void RemoveInspector()
	{
		_inspector.Despawn();
		_inspector = null;
	}
}
