using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEditor.PlayerSettings;

namespace Spellect
{
    public class SpellcastingController : MonoBehaviour
    {

        public class SpellCastEventArgs : EventArgs { public CastedSpell spell;}
        public delegate void OnSpellCastEvent(object source, SpellCastEventArgs e);
        public event OnSpellCastEvent OnSpellCast;


        public bool _inSpellMode = true;
        public List<SpellImage> _spellImages = new();
        public List<CastedSpell> Spells = new();
        private SpellImage _activeSpell;

        [SerializeField] private GameObject _drawingPointPrefab;
        [SerializeField] private GameObject _drawingConnectionPrefab;
        [SerializeField] private GameObject _spellPointPrefab;
        [SerializeField] private GameObject _spellConnectionPrefab;
        [SerializeField] private SpellDrawing _drawing;
        [SerializeField] private Material _startMaterial;
        [SerializeField] private Material _doneMaterial;

        public List<float> SpellScores = new();
        private bool _isCasting = false;

        [SerializeField] private const float DRAWING_Z = 1;
        [SerializeField] private const float MIN_DIST = 0.1f;
        [SerializeField] private const float MIN_TIME = 0.0f;
        [SerializeField] private const float FAIL_SCORE = 1f;

        private List<GameObject> _drawingPoints = new();
        private List<GameObject> _drawingConnections = new();
        private float _timeLastPointDrawn = 0f;

        public float castingTime = 2;
        private float _castingStartTime = 0;
        private bool failedDrawing = false;


        // Start is called before the first frame update
        void Start()
        {
            _drawing = new SpellDrawing();

            _spellImages.Add(new SpellImage(ImageCreator.CreateDash(), _spellPointPrefab, _spellConnectionPrefab, _startMaterial,_doneMaterial, this, Spells[0]));
            _spellImages[0].Draw();
            _spellImages[0].Hide();
            _activeSpell = _spellImages[0];
        }




        // Update is called once per frame
        void Update()
        {
            if (!_inSpellMode && Input.GetKeyDown(KeyCode.Space) )
            {
                _inSpellMode = true;
                _activeSpell.Show();
                _castingStartTime = Time.time;
                for (int i = 0; i < SpellScores.Count; i++)                    
                {
                    SpellScores[i] = 100f;
                }
            }


            if (_inSpellMode && Input.GetMouseButton(0))
            {
                Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                Vector2 pointPos = mouseWorldPos;

                if (_isCasting)
                {
                    if (CanDrawNewPoint(mouseWorldPos))
                    {
                        DrawDrawingPoint(pointPos);
                    }
                }
                else
                {
                    DrawDrawingPoint(pointPos);
                }
                _isCasting = true;
            }
            if (_inSpellMode && Time.time > _castingStartTime + castingTime)
            {
                StopCasting();
            }

            if (Input.GetMouseButtonUp(0) && _isCasting)
            {                
                StopCasting();                
            }
        }

        private void StopCasting()
        {
            _drawing.Clear();
            foreach( GameObject point in _drawingPoints)
            {
                Destroy(point);
            }
            foreach (GameObject con in _drawingConnections)
            {
                Destroy(con);
            }
            _activeSpell.Hide();
            _isCasting = false;
            _inSpellMode = false;
            _activeSpell.Hide();
        }

        private bool CanDrawNewPoint(Vector2 pos)
        {
            if (_drawing.EnoughDistanceFromLastPoint(pos) && Time.time - _timeLastPointDrawn > MIN_TIME)
            {
                return true;
            }
            return false;
        }
        private void DrawDrawingPoint(Vector2 pos)
        {
            _drawingPoints.Add(DrawPoint(pos, _drawingPointPrefab));
            _drawing.AddPoint(pos);
            _timeLastPointDrawn = Time.time;
            if (_drawing.GetNumPoints() > 1)
            {
                _drawingConnections.Add(DrawConnection(_drawingPoints[^1].transform.position, _drawingPoints[^2].transform.position, _drawingConnectionPrefab));
            }
            UpdateDrawingScores();
            CheckCompleted();
        }

        private void CheckCompleted()
        {
            for (int i = 0; i < _spellImages.Count; i++)
            {
                if (_spellImages[i].IsCompleted())
                {
                    OnSpellCast?.Invoke(this, new SpellCastEventArgs { spell = _spellImages[i].Spell });
                    StopCasting();
                }
            }
        }
        public GameObject DrawPoint(Vector2 pos, GameObject prefab)
        {
            return Instantiate(prefab, new Vector3(pos.x, pos.y, DRAWING_Z), Quaternion.identity, transform);            
        }
        public GameObject DrawConnection(Vector3 pos0, Vector3 pos1, GameObject prefab)
        {
            GameObject connection = Instantiate(prefab, (pos0 + pos1) / 2, Quaternion.identity, transform);
            Vector3 LC = connection.transform.localScale;
            connection.transform.localScale = new Vector3(Vector3.Distance(pos0,pos1), LC.y, LC.z);
            connection.transform.Rotate(new Vector3(0, 0, Vector3.SignedAngle(Vector3.right, (-pos0 + pos1), Vector3.forward)));
            return connection;
        }
        private void UpdateDrawingScores()
        {
            for (int i = 0; i < _spellImages.Count; i++)
            {
                for (int j = _drawing.FirstUnevaluatedPoint; j < _drawing.GetNumPoints(); j++)
                {
                    SpellScores[i] = SpellScores[i] - _spellImages[i].GetScore(_drawing.GetPoint(j));
                    Debug.Log("Spell score is " + SpellScores[i]);
                }
                _drawing.FirstUnevaluatedPoint = _drawing.GetNumPoints();
            }
        }
    }
}