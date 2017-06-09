using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Unigine;

namespace UnigineApp
{
	
	class AppEditorLogic : EditorLogic
	{
		// Editor logic, it takes effect only when the UnigineEditor is loaded.
		// These methods are called right after corresponding editor script's (UnigineScript) methods.

		public AppEditorLogic()
		{
		}

		public override int init()
		{
			// Write here code to be called on editor initialization.

			return 1;
		}

		// start of the main loop
		public override int update()
		{
			// Write here code to be called on editor initialization.

			return 1;
		}

		public override int render()
		{
			// Write here code to be called before rendering each render frame when editor is loaded.

			return 1;
		}
		// end of the main loop

		public override int shutdown()
		{
			// Write here code to be called on editor shutdown.

			return 1;
		}

		public override int destroy()
		{
			// Write here code to be called when the video mode is changed or the application is restarted (i.e. video_restart is called). It is used to reinitialize the graphics context.
	
			return 1;
		}

		public override int worldInit()
		{
			// Write here code to be called on world initialization when editor is loaded.
	
			return 1;
		}

		public override int worldShutdown()
		{
			// Write here code to be called on world shutdown when editor is loaded.
	
			return 1;
		}

		public override int worldSave()
		{
			// Write here code to be called on world save when editor is loaded.
	
			return 1;
		}
	}
}
