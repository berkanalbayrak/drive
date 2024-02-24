namespace ProjectDrive.EventBus
{
    public interface IEvent {}

    #region InputEvents

    public interface InputEvent : IEvent
    {
        public float Value { get; set; }
    }

    public struct ThrottleInputEvent : InputEvent
    {
        public float Value { get; set; }
    }

    public struct BrakeInputEvent : InputEvent
    {
        public float Value { get; set; }
    }

    #endregion

    #region GameEvents

    public struct StartCountdownEvent : IEvent {}

    public struct RaceStartEvent : IEvent {}

    public struct RaceEndEvent : IEvent {}

    public struct TimeSubmitEvent : IEvent
    {
        public float TimeInSeconds;
    }

    public struct PlayerCarUpdateEvent : IEvent
    {
        public float RPM;
        public float Speed;
        public int gearNumber;
    }
    #endregion
}