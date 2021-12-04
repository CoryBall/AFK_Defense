using WindowsInput.Native;
using WindowsInput;

namespace AFK_Defense
{
    public static class KeyPresser
    {
        private static readonly InputSimulator input = new();
        public static void RunInCircle()
        {
            Move(VirtualKeyCode.VK_D);
            Move(VirtualKeyCode.VK_S);
            Move(VirtualKeyCode.VK_A);
            Move(VirtualKeyCode.VK_W);
        }

        private static void Move(VirtualKeyCode key)
        {
            input.Keyboard.KeyDown(key);
            System.Threading.Thread.Sleep(250);
            input.Keyboard.KeyUp(key);
        }
    }
}
