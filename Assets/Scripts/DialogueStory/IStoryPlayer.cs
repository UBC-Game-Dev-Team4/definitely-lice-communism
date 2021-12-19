namespace DialogueStory
{
    /// <summary>
    /// Interface representing a script that can control the game story.
    /// </summary>
    public interface IStoryPlayer
    {
        /// <summary>
        /// Read-only story state that is entered by this story player.
        /// </summary>
        StoryStates State
        {
            get;
        }
    }
}