using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEditor.PlayerSettings;

namespace Spellect
{
    public class SpellcastingController : MonoBehaviour
    {
        private bool _inSpellMode = false;
        private List<SpellImage> _spellImages = new();
        [SerializeField] private GameObject _spellPointPrefab;
        [SerializeField] private GameObject _spellConnectionPrefab;

        [SerializeField] private GameObject _drawingPointPrefab;
        [SerializeField] private GameObject _drawingConnectionPrefab;

        [SerializeField] private SpellDrawing _drawing; 

        private bool _isCasting = false;

        [SerializeField] private const float DRAWING_Z = 1;
        [SerializeField] private const float MIN_DIST = 0.3f;
        [SerializeField] private const float MIN_TIME = 0.1f;

        private List<GameObject> _drawingPoints = new();
        private List<GameObject> _drawingConnections = new();
        private float _timeLastPointDrawn = 0f;


        // Start is called before the first frame update
        void Start()
        {
            _drawing = new SpellDrawing();
        }

        // Update is called once per frame
        void Update()
        {
            if (_inSpellMode && Input.GetMouseButton(0))
            {
                Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);


                if (_isCasting)
                {
                    if (TryDrawNewPoint(mouseWorldPos))
                    {
                        DrawPoint(new Vector3(mouseWorldPos.x, mouseWorldPos.y));
                    }
                }
                else
                {
                    DrawPoint(new Vector3(mouseWorldPos.x, mouseWorldPos.y));
                }

                    _isCasting = true;
            }
        }
        private bool TryDrawNewPoint(Vector2 pos)
        {
            float minDist = 10000f;

            if (_drawing.EnoughDistanceFromLastPoint(pos) && )
            {
                DrawPoint(pos);
                return true;
            }
            return false;
        }
        
        private void DrawPoint(Vector2 pos)
        {
            _drawingPoints.Add(Instantiate(_drawingPointPrefab, new Vector3(pos.x, pos.y, DRAWING_Z), Quaternion.identity, transform));
            _drawing.AddPoint(pos);
            _timeLastPointDrawn = Time.time;
            if (_drawing.GetNumPoints() > 1)
            {
                DrawConnection(_drawingPoints[^1].transform.position, _drawingPoints[^2].transform.position);
            }

        }
        private void DrawConnection(Vector3 pos0, Vector3 pos1)
        {
            Quaternion rot = Quaternion.LookRotation(Vector3.Cross(-pos0 + pos1, Vector3.up));
            _drawingConnections.Add(Instantiate(_drawingPointPrefab, (pos0+pos1)/2, rot, transform));
        }


    }
}