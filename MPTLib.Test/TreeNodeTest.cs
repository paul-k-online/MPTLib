using MPT.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MPTLib.Test
{
    
    
    /// <summary>
    ///This is a test class for TreeNodeTest and is intended
    ///to contain all TreeNodeTest Unit Tests
    ///</summary>
    [TestClass()]
    public class TreeNodeTest
    {
        [TestMethod()]
        public void TestAddChild()
        {
            const string rootValue = "treeRoot";

            var tree = new TreeNode<string>(rootValue);

            const string childValue = "child";

            tree.AddChild(childValue + 1);
            tree.AddChild(childValue + 2);
            tree.AddChild(childValue + 3);
            var child = tree.AddChild(childValue  + 4);

            

            Assert.AreEqual(tree.Value, child.Parent.Value);

            Assert.AreEqual(tree.Value, rootValue);
            Assert.AreEqual(child.Parent.Value, rootValue);


        }
    }
}
