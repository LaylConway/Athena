﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Athena.Toolbox
{
	public class Entity
	{
		private readonly List<IBehavior> _behaviors = new List<IBehavior>();
		private readonly List<Entity> _children = new List<Entity>();

		public Entity()
		{
			Behaviors = new ObservableCollection<IBehavior>(_behaviors);
			Children = new ObservableCollection<Entity>(_children);

			Children.CollectionChanged += Children_CollectionChanged;
		}

		public Entity Parent { get; private set; }
		public ObservableCollection<IBehavior> Behaviors { get; }
		public ObservableCollection<Entity> Children { get; }

		private void Children_CollectionChanged(object sender, NotifyCollectionChangedEventArgs evt)
		{
			evt.ExecuteForAdded<Entity>(e => e.Parent = this);
			evt.ExecuteForRemoved<Entity>(e => e.Parent = null);
		}

		/// <summary>
		///     Forcefully initializes all behaviors and child entitites immediately.
		/// </summary>
		public void ForceInitialize()
		{
			foreach (var child in Children)
			{
				child.ForceInitialize();
			}

			foreach (var behavior in Behaviors)
			{
				behavior.Initialize();
			}
		}
	}
}