﻿using System.Threading;
using Athena;
using Athena.Toolbox;
using NSubstitute;
using Xunit;

namespace Tests.Toolbox
{
	[Trait("Type", "Unit")]
	[Trait("Class", "Athena.Toolbox.AthenaGameLoopService")]
	public class AthenaGameLoopServiceTests
	{
		[Fact]
		public void Runtime_CreatesWindow()
		{
			// Unfortunately, windows have to be created from the thread that will be polling for updates.
			// This means the game loop service runtime will have to be responsible for opening it.

			// Arrange
			var loop = new AthenaGameLoopService();
			var window = Substitute.For<IWindowService>();
			Inject.Into(loop, new object[] {window, Substitute.For<ILoggingService>()});

			// Act
			loop.Runtime(new CancellationToken(true));

			// Assert
			window.Received(1).CreateWindow();
		}

		[Fact]
		public void Update_ProcessesEvents()
		{
			// Arrange
			var loop = new AthenaGameLoopService();
			var window = Substitute.For<IWindowService>();
			Inject.Into(loop, new object[] {window, Substitute.For<IWorldService>(), Substitute.For<ILoggingService>()});

			// Act
			loop.Update();

			// Assert
			window.Received(1).ProcessEvents();
		}
	}
}