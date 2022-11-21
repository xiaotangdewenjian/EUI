﻿namespace ET.Demo
{
    public enum SessionState
    {
        Normal,
        Game
    }

    [ComponentOf(typeof(Session))]
    public class SessionStateComponent : Entity, IAwake
    {
        public SessionState State;
    }
}