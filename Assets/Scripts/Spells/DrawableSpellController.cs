using Spellect;
using System;
using System.Collections.Generic;
using UnityEngine;
using static Spellect.SpellcastingController;

public class DrawableSpellController : MonoBehaviour
{
    public class DrawingFinishEventArgs : EventArgs { public List<Vector2> points; public CastedSpell.Type type; }
    public delegate void OnDrawingFinishEvent(object source, DrawingFinishEventArgs e);
    public event OnDrawingFinishEvent OnDrawingFinish;


    private bool _isDrawing = false;
    //private List<SpellDrawing> _drawings = new();
    private SpellDrawing _currentDrawing;
    private bool _canDraw = false;
    private float _timeLastPointDrawn = 0f;
    //private List<GameObject> _drawingPoints = new();
    //private List<GameObject> _drawingConnections = new();
    private GameObject _drawingPointPrefab;
    private GameObject _drawingConnectionPrefab;
    [SerializeField] private  float DRAWING_Z = 1;

    public List<DrawableSpellData> drawableSpells;

    private float _minTimeBetweenPoints = 0.0f;
    private float _minDistbetweenPoints = 0.0f;
    private float _drawingTime = 0f;
    private float _drawingStartTime = 0f;

    private CastedSpell _currentSpell;
    public Texture2D magicMissileCursor;
    public Texture2D firewallCursor;
    public Texture2D tornadoCursor;


    public void StartDrawing(object o, SpellCastEventArgs e)
    {
        foreach (DrawableSpellData spellData in drawableSpells)
        {
            if (e.spell.type == spellData.type)
            {
                _currentSpell = e.spell;
                _drawingStartTime = Time.time;
                _drawingTime = e.spell.strength;
                _minDistbetweenPoints = spellData.distanceBetween;
                _minTimeBetweenPoints = spellData.timeBetween;
                _drawingPointPrefab = spellData.objectPrefab;
                _canDraw = true;
                
            }
        }
    }



    // Update is called once per frame
    void Update()
    {
        if (_canDraw)
        {
            if (_currentSpell.type == CastedSpell.Type.FireWall)
            {
                Cursor.SetCursor(firewallCursor, Vector2.zero, CursorMode.Auto);

            }
            else if (_currentSpell.type == CastedSpell.Type.MagicMissile)
            {
                Cursor.SetCursor(magicMissileCursor, Vector2.zero, CursorMode.Auto);
            }
            else if (_currentSpell.type == CastedSpell.Type.Tornado)
            {
                Cursor.SetCursor(tornadoCursor, Vector2.zero, CursorMode.Auto);
            }
        }
        if (_canDraw && (Time.time > _drawingTime + _drawingStartTime || (_isDrawing && Input.GetMouseButtonUp(1) && (_currentSpell.type == CastedSpell.Type.MagicMissile || _currentSpell.type == CastedSpell.Type.Tornado))))
        {
            Debug.Log("Stopping spell after " + Time.time + " started at " + _drawingStartTime + " duration was meant to be " + _drawingTime);
            _canDraw = false;
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            if (_currentSpell.type == CastedSpell.Type.MagicMissile || _currentSpell.type  ==  CastedSpell.Type.Tornado)
            {
                Debug.Log("Finished drawing");
                OnDrawingFinish?.Invoke(this, new DrawingFinishEventArgs { points = _currentDrawing.GetPoints(), type = _currentSpell.type });
            }
        }

        if (_isDrawing && Input.GetMouseButtonUp(1))
        {
            _isDrawing = false;
            _currentDrawing = null;
            Debug.Log("Stopped drawing");
        }    

        if (_canDraw && Input.GetMouseButtonDown(1))
        {
            _isDrawing = true;
        }

        if (_canDraw && _isDrawing && Input.GetMouseButton(1))
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Vector2 pointPos = mouseWorldPos;
            if (_currentDrawing == null)
            {
                _currentDrawing = new();
                DrawDrawingPoint(pointPos);
            }
            else if (CanDrawNewPoint(mouseWorldPos, _minDistbetweenPoints))
            {
                if (_currentSpell.type == CastedSpell.Type.FireWall)
                {
                    Vector2 oldPoint = _currentDrawing.GetPoint(_currentDrawing.GetNumPoints() - 1);
                    Vector2 newPoint = oldPoint+(-_currentDrawing.GetPoint(_currentDrawing.GetNumPoints() - 1) + pointPos).normalized * _minDistbetweenPoints;
                    DrawDrawingPoint(newPoint);
                }
                else
                {
                    DrawDrawingPoint(pointPos);
                }
            }
            _isDrawing = true;
        }

        if (!_canDraw && _currentDrawing != null)
        {
            ClearDrawing();
        }
        
    }
    private void ClearDrawing()
    {
        _currentDrawing.Clear();
        _currentDrawing = null;

    }
    private bool CanDrawNewPoint(Vector2 pos, float dist)
    {
        if (_currentDrawing.EnoughDistanceFromLastPoint(pos, dist) && Time.time - _timeLastPointDrawn > _minTimeBetweenPoints)
        {
            return true;
        }
        return false;
    }

    private void DrawDrawingPoint(Vector2 pos)
    {
        DrawPoint(pos, _drawingPointPrefab);
        _currentDrawing.AddPoint(pos);
        _timeLastPointDrawn = Time.time;
        /*if (_currentDrawing.GetNumPoints() > 1 && _currentSpell.type == CastedSpell.Type.MagicMissile)
        {
            DrawConnection(_currentDrawing.GetPoints()[^1], _currentDrawing.GetPoints()[^1], _drawingConnectionPrefab));
        }*/
        //UpdateDrawingScores();
        //CheckCompleted();
    }
    public GameObject DrawPoint(Vector2 pos, GameObject prefab)
    {
        return Instantiate(prefab, new Vector3(pos.x, pos.y, DRAWING_Z), Quaternion.identity);
    }


    public GameObject DrawConnection(Vector3 pos0, Vector3 pos1, GameObject prefab)
    {
        GameObject connection = Instantiate(prefab, (pos0 + pos1) / 2, Quaternion.identity);
        Vector3 LC = connection.transform.localScale;
        connection.transform.localScale = new Vector3(Vector3.Distance(pos0, pos1), LC.y, LC.z);
        connection.transform.Rotate(new Vector3(0, 0, Vector3.SignedAngle(Vector3.right, (-pos0 + pos1), Vector3.forward)));
        return connection;
    }
}
