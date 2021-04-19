namespace Tetris
{
    public class AutoPlay
    {
        public AutoPlay(Tetris tetris)
        {
            tetris.ConnectingToAI += FindOptimalPos;
        }
        private void FindOptimalPos(object sender, TetrisEventArgs e)
        {
            
        }
    }
}