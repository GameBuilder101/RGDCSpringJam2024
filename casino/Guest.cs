using Godot;
using System;

public partial class Guest : Sprite2D
{
	[Export] // Initial node assigned in editor
	public GuestMovementNode Destination { get; set; }
	public Vector2 DestinationOffset { get; private set; }
	public bool AtDestination
	{
		get
		{
			return Position.X == Destination.Position.X + DestinationOffset.X &&
				Position.Y == Destination.Position.Y + DestinationOffset.Y;
        }
	}

	public GuestMovementNode PrevDestination { get; private set; }

	[Export]
	private float _maxDestinationOffset;

    [Export]
    private float _minMoveSpeed;
    [Export]
    private float _maxMoveSpeed;
	public float MoveSpeed { get; private set; }

    [Export]
	private double _minLoiterTime;
	[Export]
	private double _maxLoiterTime;
	public double LoiterTime { get; private set; }
	private double _currentLoiterTime;

	[Export]
	private float _idleAnimScale = 1.0f;

	private Vector2 _defaultOffset;
	private double _animTime;

	private Random _random;

	private double _despawnTime = 4.0;
	private double _currentDespawnTime = -1.0;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		_random = new Random();

        PrevDestination = Destination;
		Position = Destination.Position;

		_defaultOffset = Offset;
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        _animTime += delta;

        // Walk towards destination
        Position = Position.MoveToward(Destination.Position + DestinationOffset, MoveSpeed * (float)delta);

		if (AtDestination)
		{
			_currentLoiterTime += delta;
			if (_currentLoiterTime >= LoiterTime)
				SetNextRandomDestination();
			else
				AnimateIdle();
		}
		else
			AnimateMoving();

		if (_currentDespawnTime >= 0.0)
		{
			Modulate = new Color(1.0f, 1.0f, 1.0f, 1.0f - (float)(_currentDespawnTime / _despawnTime));

			_currentDespawnTime += delta;
			if (_currentDespawnTime > _despawnTime)
                QueueFree(); // DESTROY HAHAHAHAHHAHAHAHA DIEEEEEEEEE
        }
	}

	public void SetDestination(GuestMovementNode destination)
	{
		PrevDestination = Destination;
		Destination = destination;
		// Get a random position offset from the destination by a maximum amount
		DestinationOffset = Vector2.FromAngle((float)(_random.NextDouble() * 2.0 * Math.PI))
			* (float)_random.NextDouble() * _maxDestinationOffset;

		MoveSpeed = _minMoveSpeed + (float)_random.NextDouble() * (_maxMoveSpeed - _minMoveSpeed);
        LoiterTime = _minLoiterTime + (float)_random.NextDouble() * (_maxLoiterTime - _minLoiterTime);
		_currentLoiterTime = 0.0;

		_animTime = 0.0;
    }

	protected void SetNextRandomDestination()
	{
		// Get a destination that is not the previous
        GuestMovementNode dest = null;
		while (dest == null || dest == PrevDestination)
			dest = (GuestMovementNode)Destination.Connected[_random.Next(Destination.Connected.Length)];

		SetDestination(dest);
	}

	private void AnimateMoving()
	{
		Offset = new Vector2(_defaultOffset.X, _defaultOffset.Y - (float)Math.Sin(_animTime * 8.0) * MoveSpeed * 0.05f);
		RotationDegrees = (float)(Math.Sin(_animTime * 8.0) - 0.5) * MoveSpeed * 0.11f;
		Scale = Vector2.One;
	}

	private void AnimateIdle()
	{
		Offset = _defaultOffset;
		Rotation = 0.0f;
		Scale = new Vector2(1.0f + (float)Math.Sin(_animTime * 3.0) * 0.12f * _idleAnimScale,
			1.0f - (float)Math.Sin(_animTime * 3.0) * 0.08f * _idleAnimScale);
	}

	public void Despawn()
	{
		_currentDespawnTime = 0.0;
	}
}
