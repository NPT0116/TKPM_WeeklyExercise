using System;

namespace BE.Config;

public class StudentStatusTransitionConfig
{
    public Dictionary<int, List<int>> AllowedTransitions { get; set; } = new Dictionary<int, List<int>>();

}
