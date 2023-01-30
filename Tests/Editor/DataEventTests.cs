using com.enemyhideout.noonien;
using NUnit.Framework;

namespace Tests.Editor
{
  [TestFixture]
  public class DataEventTests
  {
    private const string TestId = "Test";
    
    [Test]
    public void TestEventQueue()
    {
      EventBuffer buffer = new EventBuffer(null);
      var testEvent = new DataEvent(TestId);
      buffer.EnqueueEvent(testEvent);
      var evt = buffer.EventForId<DataEvent>(TestId);
      Assert.That(evt, Is.EqualTo(testEvent));
      var nullEvt = buffer.EventForId<TestEvent>(TestId);
      Assert.That(nullEvt, Is.Null);
      var nullEvtFromBadId = buffer.EventForId<TestEvent>("Nothing");
      Assert.That(nullEvtFromBadId, Is.Null);

      buffer.Clear();
      evt = buffer.EventForId<DataEvent>(TestId);
      Assert.That(evt, Is.Null);
    }
  }

  public class TestEvent : DataEvent
  {
    public TestEvent(string id) : base(id)
    {
    }
  }
}