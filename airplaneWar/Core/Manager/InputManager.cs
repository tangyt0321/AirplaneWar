using System;
using System.Collections.Generic;
using System.Numerics;
using System.Windows.Forms;

namespace airplaneWar.Core.Manager.InputManager
{
    public sealed class InputManager : IDisposable
    {
        #region 单例模式
        private static readonly Lazy<InputManager> _instance =
            new Lazy<InputManager>(() => new InputManager());
        public static InputManager Instance => _instance.Value;
        //写法2
        //private static InputManager _instance;
        //public static InputManager Instance
        //{
        //    get
        //    {
        //        if (_instance == null)
        //        {
        //            _instance = new InputManager();
        //        }
        //        return _instance;
        //    }
        //}
        #endregion

        #region 输入状态数据结构
        private class InputState
        {
            public bool IsDown;
            public bool WasDown;
            public float Duration; // 按键持续时间（秒）
        }

        private readonly Dictionary<Keys, InputState> _keyStates = new Dictionary<Keys, InputState>();//键盘状态
        private readonly Dictionary<MouseButtons, InputState> _mouseButtonStates = new Dictionary<MouseButtons, InputState>();//鼠标状态
        #endregion

        #region 公共属性
        public Vector2 MousePosition { get; private set; }
        public int MouseWheelDelta { get; private set; }
        public float MouseSensitivity { get; set; } = 1.0f;
        #endregion

        #region 初始化与销毁
        private Control _inputControl;

        public void Initialize(Control control)
        {
            //if (_inputControl != null) return;

            _inputControl = control ?? throw new ArgumentNullException(nameof(control));

            Console.WriteLine(_inputControl);
            // 确保接收键盘事件
            _inputControl.TabStop = true;
            _inputControl.Focus();

            // 键盘事件
            _inputControl.KeyDown += OnKeyDown;
            _inputControl.KeyUp += OnKeyUp;

            // 鼠标事件
            _inputControl.MouseMove += OnMouseMove;
            _inputControl.MouseDown += OnMouseDown;
            _inputControl.MouseUp += OnMouseUp;
            _inputControl.MouseWheel += OnMouseWheel;



        }

        public void Dispose()
        {
            if (_inputControl == null) return;

            _inputControl.KeyDown -= OnKeyDown;
            _inputControl.KeyUp -= OnKeyUp;
            _inputControl.MouseMove -= OnMouseMove;
            _inputControl.MouseDown -= OnMouseDown;
            _inputControl.MouseUp -= OnMouseUp;
            _inputControl.MouseWheel -= OnMouseWheel;

            _inputControl = null;
        }
        #endregion

        #region 更新循环
        public void Update(double deltaTime)
        {
            // 更新按键持续时间
            foreach (var state in _keyStates.Values)
            {
                if (state.IsDown)
                    state.Duration += (float)deltaTime;
                else state.Duration = 0;
            }

            // 重置瞬时状态
            foreach (var state in _keyStates.Values)
            {
                state.WasDown = state.IsDown;
            }
            foreach (var state in _mouseButtonStates.Values)
            {
                state.WasDown = state.IsDown;
            }

            MouseWheelDelta = 0; // 每帧重置滚轮增量
        }
        #endregion

        #region 输入查询接口
        // 键盘状态查询
        public bool GetKey(Keys key) => GetKeyState(key).IsDown;
        public bool GetKeyDown(Keys key) => GetKeyState(key).IsDown && !GetKeyState(key).WasDown;
        public bool GetKeyUp(Keys key) => !GetKeyState(key).IsDown && GetKeyState(key).WasDown;
        public float GetKeyDuration(Keys key) => GetKeyState(key).Duration;

        // 鼠标按钮状态查询
        public bool GetMouseButton(MouseButtons button) => GetMouseState(button).IsDown;
        public bool GetMouseButtonDown(MouseButtons button) => GetMouseState(button).IsDown && !GetMouseState(button).WasDown;
        public bool GetMouseButtonUp(MouseButtons button) => !GetMouseState(button).IsDown && GetMouseState(button).WasDown;

        // 输入动作映射
        public bool GetAction(string actionName)
        {
            // 示例：从配置读取按键绑定
            var keys = InputMapping.GetActionKeys(actionName);
            foreach (var key in keys)
            {
                if (GetKey(key)) return true;
            }
            return false;
        }
        #endregion

        #region 事件处理
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            var state = GetKeyState(e.KeyCode);
            if (!state.IsDown)
            {
                state.Duration = 0;
                state.IsDown = true;
            }
            e.Handled = true; // 阻止系统处理
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            GetKeyState(e.KeyCode).IsDown = false;
            e.Handled = true;
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            MousePosition = new Vector2(
                (int)(e.X * MouseSensitivity),
                (int)(e.Y * MouseSensitivity));
        }

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            GetMouseState(e.Button).IsDown = true;
        }

        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            GetMouseState(e.Button).IsDown = false;
        }

        private void OnMouseWheel(object sender, MouseEventArgs e)
        {
            MouseWheelDelta = e.Delta;
        }
        #endregion

        #region 辅助方法
        private InputState GetKeyState(Keys key)
        {
            if (!_keyStates.TryGetValue(key, out var state))
            {
                state = new InputState();
                _keyStates[key] = state;
            }
            return state;
        }

        private InputState GetMouseState(MouseButtons button)
        {
            if (!_mouseButtonStates.TryGetValue(button, out var state))
            {
                state = new InputState();
                _mouseButtonStates[button] = state;
            }
            return state;
        }
        #endregion

        #region 输入映射配置类
        public static class InputMapping
        {
            private static readonly Dictionary<string, Keys[]> _actionMap = new Dictionary<string, Keys[]>
            {
                ["MoveUp"] = new[] { Keys.W, Keys.Up },
                ["MoveDown"] = new[] { Keys.S, Keys.Down },
                ["MoveLeft"] = new[] { Keys.A, Keys.Left },
                ["MoveRight"] = new[] { Keys.D, Keys.Right },
                //["Fire"] = new[] { Keys.Space, Keys.ControlKey },
                ["Pause"] = new[] { Keys.Escape }
            };

            public static void BindAction(string actionName, params Keys[] keys)
            {
                _actionMap[actionName] = keys;
            }

            public static Keys[] GetActionKeys(string actionName)
            {
                return _actionMap.TryGetValue(actionName, out var keys) ? keys : Array.Empty<Keys>();
            }
        }
        #endregion
    }
}