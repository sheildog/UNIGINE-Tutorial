using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Unigine;

namespace UnigineApp
{
	class AppSystemLogic : SystemLogic
	{
		// System logic, it exists during the application life cycle.
		// These methods are called right after corresponding system script's (UnigineScript) methods.

		public AppSystemLogic()
		{
		}

		public override int init()
		{
			// Write here code to be called on engine initialization.

			return 1;
		}

		// start of the main loop
		public override int update()
		{
			// Write here code to be called before updating each render frame.

			return 1;
		}

		public override int render()
		{
			// Write here code to be called before rendering each render frame.

			return 1;
		}
		// end of the main loop

		public override int shutdown()
		{
			// Write here code to be called on engine shutdown.

			return 1;
		}

		public override int destroy()
		{
			// Write here code to be called when the video mode is changed or the application is restarted (i.e. video_restart is called). It is used to reinitialize the graphics context.

			return 1;
		}
	}
}
