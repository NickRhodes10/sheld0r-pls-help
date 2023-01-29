using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace EffectSystem
{
    [CustomEditor(typeof(ProjectileEffect))]
    public class ProjectileEffect_Editor : Editor
    {
        private ProjectileEffect _curPE;

        private void OnEnable()
        {
            _curPE = target as ProjectileEffect;
        }

        public override void OnInspectorGUI()
        {
            _curPE.doesComeBack = EditorGUILayout.Toggle("Does Come back", _curPE.doesComeBack);

            if (_curPE.doesComeBack == true)
            {
                _curPE.comeBackTime = EditorGUILayout.FloatField("Come back after seconds", _curPE.comeBackTime);
                _curPE.explodes = false;
            }

            if (_curPE.doesComeBack == false)
            {
                _curPE.explodes = EditorGUILayout.Toggle("Explodes?: ", _curPE.explodes);

                if (_curPE.explodes)
                {
                    _curPE.ExplosionPrefab = EditorGUILayout.ObjectField("Explosion Prefab: ", _curPE.ExplosionPrefab, typeof(GameObject), true) as GameObject;
                    _curPE.explodeRadius = EditorGUILayout.FloatField("Explosion Radius: ", _curPE.explodeRadius);
                    _curPE.explosionTime = EditorGUILayout.FloatField("Explosion Time", _curPE.explosionTime);
                }
            }

           

            _curPE.hasTravelTime = EditorGUILayout.Toggle("Travel Time:", _curPE.hasTravelTime);

            if(_curPE.hasTravelTime == true)
            {
                _curPE.TravelTime = EditorGUILayout.FloatField("Time: ", _curPE.TravelTime);
            }


            _curPE.speed = EditorGUILayout.FloatField("Speed", _curPE.speed);

            _curPE.objectPool = EditorGUILayout.Vector3Field("Object Pool", _curPE.objectPool);

            _curPE.projectileObject = EditorGUILayout.ObjectField("Item", _curPE.projectileObject, typeof(GameObject), true) as GameObject;
            _curPE.direction = (ProjectileEffect.Direction)EditorGUILayout.EnumPopup("Direction", _curPE.direction);

            EditorUtility.SetDirty(_curPE);
        }
    }
}