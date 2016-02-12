using System;
using System.Collections.ObjectModel;

using Microsoft.Practices.ObjectBuilder2;

namespace TodoSystem.Service.Tools.UnityExtension
{
    internal class BuildTreeItemNode
    {
        public BuildTreeItemNode(
            NamedTypeBuildKey buildKey,
            bool nodeCreatedByContainer,
            BuildTreeItemNode parentNode)
        {
            BuildKey = buildKey;
            NodeCreatedByContainer = nodeCreatedByContainer;
            Parent = parentNode;
            Children = new Collection<BuildTreeItemNode>();
        }

        public NamedTypeBuildKey BuildKey { get; private set; }

        public Collection<BuildTreeItemNode> Children { get; private set; }

        public WeakReference ItemReference { get; private set; }

        public bool NodeCreatedByContainer { get; private set; }

        public BuildTreeItemNode Parent { get; private set; }

        public void AssignInstance(object instance)
        {
            if (ItemReference != null)
            {
                if (ItemReference.Target == instance)
                {
                    return;
                }

                throw new InvalidOperationException("Instance is already assigned.");
            }

            ItemReference = new WeakReference(instance);
        }
    }
}