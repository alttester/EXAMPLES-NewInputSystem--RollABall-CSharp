using System.Threading;
using Altom.AltUnityDriver;
using NUnit.Framework;
using UnityEngine;

public class Tests_AUT
{
    public AltUnityDriver altUnityDriver;
    //Before any test it connects with the socket
    [OneTimeSetUp]
    public void SetUp()
    {
        altUnityDriver = new AltUnityDriver();
    }

    //At the end of the test closes the connection with the socket
    [OneTimeTearDown]
    public void TearDown()
    {
        altUnityDriver.Stop();
    }

    [Test]
    public void TestMoveBallWithMoveMouse()
    {
        altUnityDriver.LoadScene("MiniGame");
        var ball = altUnityDriver.FindObject(By.NAME, "Player");
        var initialPosition = ball.getWorldPosition();
        altUnityDriver.MoveMouse(new AltUnityVector2(0, 0), 3f);
        altUnityDriver.MoveMouse(new AltUnityVector2(100, 100), 3f);
        altUnityDriver.MoveMouse(new AltUnityVector2(-200, -200), 3f);
        ball = altUnityDriver.FindObject(By.NAME, "Player");
        var finalPosition = ball.getWorldPosition();
        Assert.AreNotEqual(initialPosition.x, finalPosition.x);
    }

    [Test]
    public void TestScrollOnScrollbar()
    {
        altUnityDriver.LoadScene("MiniGame");
        var scrollbar = altUnityDriver.FindObject(By.NAME, "Scrollbar Vertical");
        var scrollbarPosition = scrollbar.GetComponentProperty<float>("UnityEngine.UI.Scrollbar", "value", "UnityEngine.UI");
        altUnityDriver.MoveMouse(altUnityDriver.FindObject(By.NAME, "Scroll View").getScreenPosition(), 1);
        altUnityDriver.Scroll(new AltUnityVector2(-3000, -3000), 1, true);
        var scrollbarPositionFinal = scrollbar.GetComponentProperty<float>("UnityEngine.UI.Scrollbar", "value", "UnityEngine.UI");
        Assert.AreNotEqual(scrollbarPosition, scrollbarPositionFinal);
    }

    [Test]
    public void TestMoveMouseOnScrollbar()
    {
        altUnityDriver.LoadScene("MiniGame");
        var objects = altUnityDriver.GetAllElementsLight();
        var scrollbar = altUnityDriver.WaitForObject(By.NAME, "Handle");
        AltUnityVector2 scrollbarInitialPosition = scrollbar.getScreenPosition(); // use screen coordinates instead of world coordinates        
        altUnityDriver.MoveMouse(scrollbar.getScreenPosition()); // move mouse in area where scroll reacts
        altUnityDriver.Scroll(-200, 0.1f);
        scrollbar = altUnityDriver.WaitForObject(By.NAME, "Handle");
        AltUnityVector2 scrollbarFinalPosition = scrollbar.getScreenPosition();
        Assert.AreNotEqual(scrollbarInitialPosition.y, scrollbarFinalPosition.y);//compare y as there is no equality comparer on AltUnityVector2. and we expect only y to change
    }

    [Test]
    public void TestSwipeOnScrollbar()
    {
        altUnityDriver.LoadScene("MiniGame");
        var scrollbar = altUnityDriver.WaitForObject(By.NAME, "Handle");
        AltUnityVector2 scrollbarInitialPosition = new AltUnityVector2(scrollbar.worldX, scrollbar.worldY);
        altUnityDriver.Swipe(new AltUnityVector2(scrollbar.x, scrollbar.y), new AltUnityVector2(scrollbar.x, scrollbar.y - 200), 3);
        scrollbar = altUnityDriver.WaitForObject(By.NAME, "Handle");
        AltUnityVector2 scrollbarFinalPosition = new AltUnityVector2(scrollbar.worldX, scrollbar.worldY);
        Assert.AreNotEqual(scrollbarInitialPosition.y,scrollbarFinalPosition.y);
    }

    [Test]
    public void TestClickNearScrollBarMovesScrollBar()
    {
        altUnityDriver.LoadScene("MiniGame");

        var scrollbar = altUnityDriver.WaitForObject(By.NAME, "Handle");
        var scrollbarInitialPosition = scrollbar.getScreenPosition();
        
        var scrollBarMoved = new AltUnityVector2(scrollbar.x, scrollbar.y - 100);
        altUnityDriver.MoveMouse(scrollBarMoved, 1);

        altUnityDriver.Click(new AltUnityVector2(scrollbar.x, scrollbar.y - 100));

        scrollbar = altUnityDriver.WaitForObject(By.NAME, "Handle");
        var scrollbarFinalPosition = scrollbar.getScreenPosition();

        Assert.AreNotEqual(scrollbarInitialPosition.y, scrollbarFinalPosition.y);
    }

    [Test]
    public void TestBeginMoveEndTouchMovesScrollbar()
    {
        altUnityDriver.LoadScene("MiniGame");
        var scrollbar = altUnityDriver.FindObject(By.NAME, "Handle");
        var scrollbarPosition = scrollbar.getScreenPosition();
        int fingerId = altUnityDriver.BeginTouch(scrollbar.getScreenPosition());
        AltUnityVector2 newPosition = new AltUnityVector2(scrollbar.x, scrollbar.y - 500);
        altUnityDriver.MoveTouch(fingerId, newPosition);
        altUnityDriver.EndTouch(fingerId);
        scrollbar = altUnityDriver.FindObject(By.NAME, "Handle");
        var scrollbarPositionFinal = scrollbar.getScreenPosition();

        Assert.AreNotEqual(scrollbarPosition.y, scrollbarPositionFinal.y);
    }

    [Test]
    public void TestPressKeyNearScrollBarMovesScrollBar()
    {
        altUnityDriver.LoadScene("MiniGame");

        var scrollbar = altUnityDriver.FindObject(By.NAME, "Handle");
        var scrollbarPosition = scrollbar.getScreenPosition();
        var scrollBarMoved = new AltUnityVector2(scrollbar.x, scrollbar.y - 100);
        altUnityDriver.MoveMouse(scrollBarMoved, 1);
        altUnityDriver.PressKey(AltUnityKeyCode.Mouse0, 0.1f);
        scrollbar = altUnityDriver.FindObject(By.NAME, "Handle");
        var scrollbarPositionFinal = scrollbar.getScreenPosition();
        Assert.AreNotEqual(scrollbarPosition.y, scrollbarPositionFinal.y);
    }

    [Test]
    public void TestKeyDownAndKeyUpMouse0MovesScrollBar()
    {
        altUnityDriver.LoadScene("MiniGame");

        var scrollbar = altUnityDriver.FindObject(By.NAME, "Scrollbar Vertical");
        var handle = altUnityDriver.FindObject(By.NAME, "Handle");
        var scrollbarPosition = scrollbar.GetComponentProperty<float>("UnityEngine.UI.Scrollbar", "value", "UnityEngine.UI");
        var scrollBarMoved = new AltUnityVector2(handle.x, handle.y - 100);
        altUnityDriver.MoveMouse(scrollBarMoved, 1);
        altUnityDriver.KeyDown(AltUnityKeyCode.Mouse0);
        altUnityDriver.KeyUp(AltUnityKeyCode.Mouse0);
        scrollbar = altUnityDriver.FindObject(By.NAME, "Scrollbar Vertical");
        var scrollbarPositionFinal = scrollbar.GetComponentProperty<float>("UnityEngine.UI.Scrollbar", "value", "UnityEngine.UI");
        Assert.AreNotEqual(scrollbarPosition, scrollbarPositionFinal);
    }

    [Test]
    public void TestBallMovesOnPressKeys()
    {
        altUnityDriver.LoadScene("MiniGame");

        var ball = altUnityDriver.FindObject(By.NAME, "Player");
        altUnityDriver.PressKey(AltUnityKeyCode.S, 1f, 1f);
        var newBall = altUnityDriver.FindObject(By.NAME, "Player");
        Debug.Log("Ball moved backward");
        Assert.AreNotEqual(ball.getWorldPosition().z, newBall.getWorldPosition().z);
        Thread.Sleep(1000);

        ball = altUnityDriver.FindObject(By.NAME, "Player");
        altUnityDriver.PressKey(AltUnityKeyCode.W, 1f, 2f);
        newBall = altUnityDriver.FindObject(By.NAME, "Player");
        Debug.Log("Ball moved forward");
        Assert.AreNotEqual(ball.getWorldPosition().z, newBall.getWorldPosition().z);
        Thread.Sleep(1000);

        ball = altUnityDriver.FindObject(By.NAME, "Player");
        altUnityDriver.PressKey(AltUnityKeyCode.A, 1f, 2f);
        newBall = altUnityDriver.FindObject(By.NAME, "Player");
        Debug.Log("Ball moved to the left");
        Assert.AreNotEqual(ball.getWorldPosition().x, newBall.getWorldPosition().x);
        Thread.Sleep(1000);

        ball = altUnityDriver.FindObject(By.NAME, "Player");
        altUnityDriver.PressKey(AltUnityKeyCode.D, 1f, 2f);
        newBall = altUnityDriver.FindObject(By.NAME, "Player");
        Debug.Log("Ball moved to the right");
        Assert.AreNotEqual(ball.getWorldPosition().x, newBall.getWorldPosition().x);
        Thread.Sleep(2000);
    }

    [Test]
    public void TestTiltBall()
    {
        altUnityDriver.LoadScene("MiniGame");
        var ball = altUnityDriver.FindObject(By.NAME, "Player");
        var initialPosition = ball.getWorldPosition();
        altUnityDriver.Tilt(new AltUnityVector3(1000, 1000, 1), 3f);
        Assert.AreNotEqual(initialPosition.x, altUnityDriver.FindObject(By.NAME, "Player").getWorldPosition().x);
    }

    [Test]
    public void TestDoubleClick()
    {
        altUnityDriver.LoadScene("MiniGame");
        var button = altUnityDriver.FindObject(By.NAME, "SpecialButton").Click();
        Thread.Sleep(1000);
        button.Click();
        var text = altUnityDriver.FindObject(By.PATH,"//ScrollCanvas/SpecialButton/Text (TMP)").GetText();
        Assert.AreEqual("2",text);
    }
}