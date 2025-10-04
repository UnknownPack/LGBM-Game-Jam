using System;
using IngameDebugConsole;
using UnityEngine;

namespace src
{
    public class TestingCommandConsole : MonoBehaviour
    {
        void Start()
        {
            for (int i = 0; i < 4; i++)
            {
                Debug.Log($"Testing... count: {i}");
            }
            DebugLogConsole.AddCommand( "Test_One", "just prints a debug log", DebugOne );
            DebugLogConsole.AddCommand( "Test_Two", "prints a bunch of errors mate", ErrorMessages );
            DebugLogConsole.AddCommand<String, Vector2>( "CreateObject", "Creates a gameObject at the vector 2", CreateGameObject );
        }

        void DebugOne() => Debug.Log("Hey mama, I'm famous!");


        private static void CreateGameObject(String name, Vector2 position)
        {
            GameObject circle = new GameObject(name);
            circle.transform.position = new Vector3(position.x, position.y, 0f);
            Debug.Log($"GameObject named: {name} placed in {position}");
        }

        private void ErrorMessages()
        {
            Debug.LogError("Please help! Mr goon is attacking!!!");
        }
        
    }
}
