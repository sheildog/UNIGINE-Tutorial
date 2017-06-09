using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Unigine;

namespace UnigineApp
{
	class UnigineApp
	{
		static void Main(string[] args)
		{
			Wrapper.init();

			AppSystemLogic systemLogic = new AppSystemLogic();
			AppWorldLogic worldLogic = new AppWorldLogic();
			AppEditorLogic editorLogic = new AppEditorLogic();

			Engine engine = Engine.init(Engine.VERSION, args);

			engine.main(systemLogic, worldLogic, editorLogic);

			Engine.shutdown();
		}
	}
}