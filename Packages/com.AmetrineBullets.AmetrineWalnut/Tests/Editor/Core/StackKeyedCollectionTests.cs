using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace AmetrineWalnut.Tests
{
    [TestFixture]
    public class StackKeyedCollectionTests
    {
        private StackKeyedCollection<string, TestItem> _collection;

        private class TestItem
        {
            public string Id { get; set; }
            public string Name { get; set; }

            public TestItem(string id, string name)
            {
                Id = id;
                Name = name;
            }
        }

        [SetUp]
        public void SetUp()
        {
            _collection = new StackKeyedCollection<string, TestItem>(item => item.Id);
        }

        [Test]
        public void Push_AddsItemToCollection()
        {
            var item = new TestItem("1", "Item 1");
            
            _collection.Push(item);
            
            Assert.AreEqual(1, _collection.Count);
            Assert.AreEqual(item, _collection["1"]);
        }

        [Test]
        public void Push_ExistingItem_MovesToTop()
        {
            var item1 = new TestItem("1", "Item 1");
            var item2 = new TestItem("2", "Item 2");
            
            _collection.Push(item1);
            _collection.Push(item2);
            _collection.Push(item1);
            
            Assert.AreEqual(2, _collection.Count);
            Assert.AreEqual(item1, _collection.Peek());
        }

        [Test]
        public void Pop_RemovesAndReturnsLastItem()
        {
            var item1 = new TestItem("1", "Item 1");
            var item2 = new TestItem("2", "Item 2");
            
            _collection.Push(item1);
            _collection.Push(item2);
            
            var popped = _collection.Pop();
            
            Assert.AreEqual(item2, popped);
            Assert.AreEqual(1, _collection.Count);
            Assert.AreEqual(item1, _collection.Peek());
        }

        [Test]
        public void Pop_EmptyCollection_ThrowsException()
        {
            Assert.Throws<KeyNotFoundException>(() => _collection.Pop());
        }

        [Test]
        public void Peek_ReturnsLastItemWithoutRemoving()
        {
            var item = new TestItem("1", "Item 1");
            
            _collection.Push(item);
            
            var peeked = _collection.Peek();
            
            Assert.AreEqual(item, peeked);
            Assert.AreEqual(1, _collection.Count);
        }

        [Test]
        public void Peek_EmptyCollection_ThrowsException()
        {
            Assert.Throws<KeyNotFoundException>(() => _collection.Peek());
        }

        [Test]
        public void TryPeek_NonEmptyCollection_ReturnsTrueAndItem()
        {
            var item = new TestItem("1", "Item 1");
            _collection.Push(item);
            
            var result = _collection.TryPeek(out var peeked);
            
            Assert.IsTrue(result);
            Assert.AreEqual(item, peeked);
            Assert.AreEqual(1, _collection.Count);
        }

        [Test]
        public void TryPeek_EmptyCollection_ReturnsFalseAndDefault()
        {
            var result = _collection.TryPeek(out var peeked);
            
            Assert.IsFalse(result);
            Assert.IsNull(peeked);
        }

        [Test]
        public void TryPop_NonEmptyCollection_ReturnsTrueAndItem()
        {
            var item = new TestItem("1", "Item 1");
            _collection.Push(item);
            
            var result = _collection.TryPop(out var popped);
            
            Assert.IsTrue(result);
            Assert.AreEqual(item, popped);
            Assert.AreEqual(0, _collection.Count);
        }

        [Test]
        public void TryPop_EmptyCollection_ReturnsFalseAndDefault()
        {
            var result = _collection.TryPop(out var popped);
            
            Assert.IsFalse(result);
            Assert.IsNull(popped);
        }

        [Test]
        public void GetItemForKey_ExistingKey_ReturnsItem()
        {
            var item = new TestItem("1", "Item 1");
            _collection.Push(item);
            
            var retrieved = _collection.GetItemForKey("1");
            
            Assert.AreEqual(item, retrieved);
        }

        [Test]
        public void GetItemForKey_NonExistingKey_ThrowsException()
        {
            Assert.Throws<KeyNotFoundException>(() => _collection.GetItemForKey("nonexistent"));
        }

        [Test]
        public void PopForTargetItem_RemovesAllItemsUntilTarget()
        {
            var item1 = new TestItem("1", "Item 1");
            var item2 = new TestItem("2", "Item 2");
            var item3 = new TestItem("3", "Item 3");
            
            _collection.Push(item1);
            _collection.Push(item2);
            _collection.Push(item3);
            
            var popped = _collection.PopForTargetItem("2");
            
            Assert.AreEqual(item2, popped);
            Assert.AreEqual(1, _collection.Count);
            Assert.AreEqual(item1, _collection.Peek());
        }

        [Test]
        public void PopForTargetItem_NonExistingKey_ThrowsException()
        {
            var item = new TestItem("1", "Item 1");
            _collection.Push(item);
            
            Assert.Throws<KeyNotFoundException>(() => _collection.PopForTargetItem("nonexistent"));
        }

        [Test]
        public void Contains_ExistingKey_ReturnsTrue()
        {
            var item = new TestItem("1", "Item 1");
            _collection.Push(item);
            
            Assert.IsTrue(_collection.Contains("1"));
        }

        [Test]
        public void Contains_NonExistingKey_ReturnsFalse()
        {
            Assert.IsFalse(_collection.Contains("nonexistent"));
        }
    }
}