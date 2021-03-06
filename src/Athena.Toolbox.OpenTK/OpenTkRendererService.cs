﻿using System.Drawing;
using System.Threading;
using OpenTK.Graphics.OpenGL4;

namespace Athena.Toolbox.OpenTK
{
	[Service("OpenTK Rendering Service", "88E5C33F-875C-49E2-B6AD-463308998B9C")]
	public sealed class OpenTkRendererService : IService
	{
		[Depend] private Game _game;
		[Depend] private IWindowService _window;
		[Depend] private IWorldService _world;
		[Depend] private ILoggingService _logging;

		public void Initialize()
		{
			_game.RegisterRuntime(Runtime);
		}

		public void Dispose()
		{
		}

		private void Runtime(CancellationToken token)
		{
			_logging.Info("Starting Rendering Runtime...");

			while (!token.IsCancellationRequested)
			{
				// Wait until we get a signal that the world is updated
				_world.WaitForUpdate();

				// We're rendering on a different thread than the window creation
				// So we need to make the context current
				_window.MakeCurrent();

				GL.ClearColor(Color.CornflowerBlue);
				GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
				
				_window.SwapBuffers();
			}
		}
	}
}