using System.Collections;
using Droppy.InteractionSystem;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Droppy.Tests.PlayMode
{
    public class InteractionAgentTests
    {
        private const string TriggerEnterMethodName = "OnTriggerEnter2D";
        private const string TriggerExitMethodName = "OnTriggerExit2D";

        private GameObject agentGameObject;
        private InteractionAgent interactionAgent;
        private GameObject interactableGameObject;
        private FakeInteractableComponent fakeInteractableComponent;
        private Collider2D interactableCollider;

        [SetUp]
        public void SetUp()
        {
            agentGameObject = new GameObject(nameof(InteractionAgent));
            interactionAgent = agentGameObject.AddComponent<InteractionAgent>();

            interactableGameObject = new GameObject("InteractableObject");
            fakeInteractableComponent = interactableGameObject.AddComponent<FakeInteractableComponent>();
            interactableCollider = interactableGameObject.AddComponent<BoxCollider2D>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(agentGameObject);
            Object.DestroyImmediate(interactableGameObject);
        }

        [UnityTest]
        public IEnumerator Trigger_Enter_Invokes_Enter_Interaction_On_Matching_Components()
        {
            InvokePrivateMethod(interactionAgent, TriggerEnterMethodName, interactableCollider);
            yield return null;

            Assert.That(fakeInteractableComponent.EnterInteractionCalled, Is.True);
            Assert.That(fakeInteractableComponent.LastAgentPassed, Is.EqualTo(agentGameObject));
        }

        [UnityTest]
        public IEnumerator Trigger_Exit_Invokes_Exit_Interaction_On_Matching_Components()
        {
            InvokePrivateMethod(interactionAgent, TriggerEnterMethodName, interactableCollider);
            InvokePrivateMethod(interactionAgent, TriggerExitMethodName, interactableCollider);
            yield return null;

            Assert.That(fakeInteractableComponent.ExitInteractionCalled, Is.True);
            Assert.That(fakeInteractableComponent.LastAgentPassed, Is.EqualTo(agentGameObject));
        }

        [UnityTest]
        public IEnumerator Start_Interaction_Invokes_Interact_And_Start_Interaction_On_Current_Interactables()
        {
            InvokePrivateMethod(interactionAgent, TriggerEnterMethodName, interactableCollider);
            interactionAgent.StartInteraction();
            yield return null;

            Assert.That(fakeInteractableComponent.InteractCalled, Is.True);
            Assert.That(fakeInteractableComponent.StartInteractionCalled, Is.True);
            Assert.That(fakeInteractableComponent.LastAgentPassed, Is.EqualTo(agentGameObject));
        }

        [UnityTest]
        public IEnumerator End_Interaction_Invokes_End_Interaction_On_Hold_Interactables()
        {
            InvokePrivateMethod(interactionAgent, TriggerEnterMethodName, interactableCollider);
            interactionAgent.EndInteraction();
            yield return null;

            Assert.That(fakeInteractableComponent.EndInteractionCalled, Is.True);
            Assert.That(fakeInteractableComponent.LastAgentPassed, Is.EqualTo(agentGameObject));
        }

        [UnityTest]
        public IEnumerator Start_Interaction_Does_Not_Invoke_Methods_If_Object_Exited_Trigger()
        {
            InvokePrivateMethod(interactionAgent, TriggerEnterMethodName, interactableCollider);
            InvokePrivateMethod(interactionAgent, TriggerExitMethodName, interactableCollider);
            interactionAgent.StartInteraction();
            yield return null;

            Assert.That(fakeInteractableComponent.InteractCalled, Is.False);
            Assert.That(fakeInteractableComponent.StartInteractionCalled, Is.False);
        }

        private static void InvokePrivateMethod(object targetInstance, string methodName, object parameter)
        {
            System.Reflection.MethodInfo methodInformation = targetInstance.GetType().GetMethod(
                methodName,
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance
            );
            methodInformation?.Invoke(targetInstance, new[] { parameter });
        }

        private class FakeInteractableComponent : MonoBehaviour, IEnterInteractableArea, IExitInteractableArea, IInteractable, IHoldInteractable
        {
            public bool EnterInteractionCalled { get; private set; }
            public bool ExitInteractionCalled { get; private set; }
            public bool InteractCalled { get; private set; }
            public bool StartInteractionCalled { get; private set; }
            public bool EndInteractionCalled { get; private set; }
            public GameObject LastAgentPassed { get; private set; }

            public void EnterInteraction(GameObject agent)
            {
                EnterInteractionCalled = true;
                LastAgentPassed = agent;
            }

            public void ExitInteraction(GameObject agent)
            {
                ExitInteractionCalled = true;
                LastAgentPassed = agent;
            }

            public void Interact(GameObject agent)
            {
                InteractCalled = true;
                LastAgentPassed = agent;
            }

            public void StartInteraction(GameObject agent)
            {
                StartInteractionCalled = true;
                LastAgentPassed = agent;
            }

            public void EndInteraction(GameObject agent)
            {
                EndInteractionCalled = true;
                LastAgentPassed = agent;
            }
        }
    }
}