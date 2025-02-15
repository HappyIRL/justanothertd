using NUnit.Framework;
using UnityEngine;
using Assets._Project.Scripts.Unit.Base;


public class SelectionTest
{
    private GameObject unitObject;
    private GameUnit unit;

    [SetUp]
    public void Setup()
    {
		// Create a test unit GameObject before each test
		unitObject = new GameObject();
		unit = unitObject.AddComponent<GameUnit>();
	}

	[TearDown]
	public void Teardown()
	{
		// Destroy the GameObject after each test to avoid leftovers
		GameObject.DestroyImmediate(unitObject);
	}

	[Test]
	public void OnSelected_SelectingAUnit_FiresSelectionEvent()
	{
		//Arrange
		bool eventTriggered = false;
		unit.OnSelectionChanged += isSelected => eventTriggered = isSelected;

		//Act
		unit.OnSelected();

		//Assert
		Assert.IsTrue(eventTriggered, "Unit Selection event should have been triggered.");
	}

	[Test]
	public void OnDeselected_DeselectingAUnit_FiresDeselectionEvent()
	{
		// Arrange
		bool eventTriggered = false;
		unit.OnSelectionChanged += (isSelected) => { eventTriggered = !isSelected; };

		// Act
		unit.OnDeselected();

		// Assert
		Assert.IsTrue(eventTriggered, "Unit deselection event should have been triggered.");
	}
}
