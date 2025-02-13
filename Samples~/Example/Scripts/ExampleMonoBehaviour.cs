using ObservableVariables.BasicTypes;
using ObservableVariables.Unity;
using UnityEngine;

namespace ObservableVariables.Example
{
    public class ExampleMonoBehaviour : MonoBehaviour
    {
        public GameObject NewGameObject;
        
        [Header("Basic types")]
        [SerializeField] private ObservableInt _observableInt = 3;
        [SerializeField] private ObservableFloat _observableFloat = .5f;
        [SerializeField] private ObservableBool _observableBool = true;
        [Header("Unity")]
        [SerializeField] private ObservableGameObject _observableGameObject;
        [SerializeField] private ObservableTransform _observableTransform;
        [SerializeField] private ObservableBehaviour _observableBehaviour;
        [SerializeField] private ObservableVector2 _observableVector2;
        [SerializeField] private ObservableVector3 _currentPosition;

        private void Start()
        {
            _observableGameObject.ValueChanged += ObservableGameObjectOnValueChanged;

            _observableBehaviour.Value = this;
            _observableTransform.Value = FindAnyObjectByType<Transform>();
        }

        private void OnDestroy()
        {
            _observableGameObject.ValueChanged -= ObservableGameObjectOnValueChanged;
        }

        private void Update()
        {
            _currentPosition.Value = transform.position;
            
            if (NewGameObject == null)
                return;

            _observableGameObject.Value = NewGameObject;
            NewGameObject = null;
        }

        private void ObservableGameObjectOnValueChanged(GameObject newValue)
        {
            Debug.Log($"New game object selected: {newValue}");
        }
    }
}