using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
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
        private CastedSpell activeSpell;
        private SpellImage _activeSpell = null;

        [SerializeField] private GameObject _drawingPointPrefab;
        [SerializeField] private GameObject _drawingConnectionPrefab;
        [SerializeField] private GameObject _spellPointPrefab;
        [SerializeField] private GameObject _spellConnectionPrefab;
        [SerializeField] private SpellDrawing _drawing;
        [SerializeField] private Material _startMaterial;
        [SerializeField] private Material _doneMaterial;

        private List<float> SpellScores = new();
        private bool _isCasting = false;

        [SerializeField] private const float DRAWING_Z = 1;
        [SerializeField] private const float MIN_DIST = 0.1f;
        [SerializeField] private const float MIN_TIME = 0.0f;
        [SerializeField] private const float FAIL_SCORE = 1f;

        private List<GameObject> _drawingPoints = new();
        private List<GameObject> _drawingConnections = new();
        private float _timeLastPointDrawn = 0f;

        private float castingTime = 500;
        private float _castingStartTime = 0;
        private bool failedDrawing = false;


        // Start is called before the first frame update
        void Start()
        {
            _drawing = new SpellDrawing();

            SpellImage newSpell;


            // LETTER A

            /*
            newSpell = new SpellImage(ImageCreator.DrawLetterE(), _spellPointPrefab, _spellConnectionPrefab,
                _startMaterial, _doneMaterial, this, new CastedSpell { type = CastedSpell.Type.Tornado, strength = 4f });
            _spellImages.Add(newSpell);
            newSpell.Draw();
            newSpell.Hide();
            SpellScores.Add(0f);
            */
            // MAGIC MISSILE SPELL
            newSpell = new SpellImage(ImageCreator.CreateMeteor(), _spellPointPrefab, _spellConnectionPrefab,
                _startMaterial, _doneMaterial, this, new CastedSpell { type = CastedSpell.Type.MagicMissile, strength = 20f});
            _spellImages.Add(newSpell);
            newSpell.Draw();
            newSpell.Hide();
            SpellScores.Add(0f);
            // FIRE WALL SPELL
            newSpell = new SpellImage(ImageCreator.CreateFire(), _spellPointPrefab, _spellConnectionPrefab,
                _startMaterial, _doneMaterial, this, new CastedSpell { type = CastedSpell.Type.FireWall, strength = 4f });
            _spellImages.Add(newSpell);
            newSpell.Draw();
            newSpell.Hide();
            SpellScores.Add(0f);
            // ICE SPELL
            newSpell = new SpellImage(ImageCreator.CreateIce(), _spellPointPrefab, _spellConnectionPrefab,
                _startMaterial, _doneMaterial, this, new CastedSpell { type = CastedSpell.Type.Icicle, strength = 4f });
            _spellImages.Add(newSpell);
            newSpell.Draw();
            newSpell.Hide();
            SpellScores.Add(0f);

            // WIND SPELL
            newSpell = new SpellImage(ImageCreator.CreateNado(), _spellPointPrefab, _spellConnectionPrefab,
                _startMaterial, _doneMaterial, this, new CastedSpell { type = CastedSpell.Type.Tornado, strength = 4f });
            _spellImages.Add(newSpell);
            newSpell.Draw();
            newSpell.Hide();
            SpellScores.Add(0f);

            // LASER SPELL
            newSpell = new SpellImage(ImageCreator.CreateLaser(), _spellPointPrefab, _spellConnectionPrefab,
                _startMaterial, _doneMaterial, this, new CastedSpell { type = CastedSpell.Type.Laser, strength = 4f });
            _spellImages.Add(newSpell);
            newSpell.Draw();
            newSpell.Hide();
            SpellScores.Add(0f);


            //_spellImages.Add(new SpellImage(ImageCreator.CreateDash(), _spellPointPrefab, _spellConnectionPrefab, _startMaterial,_doneMaterial, this, Spells[0]));
            //_activeSpell = _spellImages[0];
        }

        public void ChangeSpell(object o, SpellbookController.BookChangedEventArgs e)
        {
            foreach (SpellImage image in _spellImages)
            {
                if (image.Spell.type == e.book.castedSpell.type)
                {
                    Debug.Log("Changed active spell to " + e.book.castedSpell.type);
                    _activeSpell = image;
                }
            }
        }


        // Update is called once per frame
        void Update()
        {
            if (_activeSpell != null && !_inSpellMode && Input.GetKeyDown(KeyCode.Space) )
            {
                _inSpellMode = true;
                _activeSpell.Show();
                _castingStartTime = Time.time;
                for (int i = 0; i < SpellScores.Count; i++)                    
                {
                    SpellScores[i] = 100f;
                }
            }


            if (_inSpellMode && Input.GetMouseButton(1))
            {
                Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                Vector2 pointPos = mouseWorldPos- transform.position;

                if (_isCasting)
                {
                    if (CanDrawNewPoint(pointPos))
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
            foreach (SpellImage image in _spellImages)
            {
                image.Hide();
            }
            _isCasting = false;
            _inSpellMode = false;
        }

        private bool CanDrawNewPoint(Vector2 pos)
        {
            if (_drawing.EnoughDistanceFromLastPoint(pos, 0.1f) && Time.time - _timeLastPointDrawn > MIN_TIME)
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
                _drawingConnections.Add(DrawConnection(_drawingPoints[^1].transform.position- transform.position, _drawingPoints[^2].transform.position - transform.position, _drawingConnectionPrefab));
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
                    Debug.Log("Successfully cast" + _spellImages[i].Spell.type);
                    OnSpellCast?.Invoke(this, new SpellCastEventArgs { spell = _spellImages[i].Spell });
                    StopCasting();
                }
            }
        }
        public GameObject DrawPoint(Vector2 pos, GameObject prefab)
        {
            return Instantiate(prefab, transform.position + new Vector3(pos.x, pos.y, DRAWING_Z), Quaternion.identity, transform);            
        }

        public GameObject DrawDrawingPoint(Vector2 pos, GameObject prefab)
        {
            return Instantiate(prefab, new Vector3(pos.x, pos.y, DRAWING_Z), Quaternion.identity, transform);
        }
        public GameObject DrawConnection(Vector3 pos0, Vector3 pos1, GameObject prefab)
        {
            GameObject connection = Instantiate(prefab, this.transform.position + (pos0 + pos1) / 2, Quaternion.identity, transform);
            Vector3 LC = connection.transform.localScale;
            connection.transform.localScale = new Vector3(Vector3.Distance(pos0,pos1), LC.y, LC.z);
            connection.transform.Rotate(new Vector3(0, 0, Vector3.SignedAngle(Vector3.right, (-pos0 + pos1), Vector3.forward)));
            return connection;
        }

        public GameObject DrawDrawingConnection(Vector3 pos0, Vector3 pos1, GameObject prefab)
        {
            GameObject connection = Instantiate(prefab,  (pos0 + pos1) / 2, Quaternion.identity, transform);
            Vector3 LC = connection.transform.localScale;
            connection.transform.localScale = new Vector3(Vector3.Distance(pos0, pos1), LC.y, LC.z);
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
                    //Debug.Log("Spell score is " + SpellScores[i]);
                }
            }
            _drawing.FirstUnevaluatedPoint = _drawing.GetNumPoints();
        }
    }
}