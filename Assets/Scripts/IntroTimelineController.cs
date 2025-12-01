using UnityEngine;
using UnityEngine.Playables;
using Droppy.Level; 

public class IntroTimelineController : MonoBehaviour
{
    public PlayableDirector director;
    public Level level;  

    void Start()
    {
        director.stopped += OnTimelineFinished;
    }

    void OnTimelineFinished(PlayableDirector d)
    {
        level.StartLevel();
    }
}
