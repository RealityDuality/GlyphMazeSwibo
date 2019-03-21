using GestureRecognizer;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;


public class DetectorManage : MonoBehaviour
{
    //Positional Variables.
    public float CurrentX;
    public float CurrentY;
    public Vector3 MouseData;
    public string StateReadX;
    public string StateReadY;
    public string MainPosText;
    public float inDeadzoneX = 0.2f;
    public float inDeadzoneY = 0.2f;

    //Not Yet Implemented
    public float outDeadzoneX;
    public float outDeadzoneY;


    //Draw attributes
    [SerializeField]
    private ModifiedDetector GetDraw;
    [SerializeField]
    private float Timer;
    [SerializeField]
    private float MaxTimer = 3f;
    public Vector2 passdata;
    public int recognized;
    public int fizzles;

    [SerializeField]
    private int DownUsed;

    public Transform referenceRoot;
 
    GesturePatternDraw[] references;

    public string lastspell;

    public List<string> SpellsCast;

    public GestureRecognizer.UILineRenderer spellLine;

    // Start is called before the first frame update
    void Start()
    {
        GetDraw = this.gameObject.GetComponent<ModifiedDetector>();
        references = referenceRoot.GetComponentsInChildren<GesturePatternDraw>();
       // spellLine = this.gameObject.GetComponentsInChildren<UILineRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        MoveCalc();
        //  passdata = GetDraw.eventData;
        //Is Draw Mode used? make if check

        //If centered
        while (StateReadX == "Centered" && StateReadY == "Centered")
        {
            //Start timer
            Timer += Time.deltaTime;
            //if Timer done, trigger drawcheck coroutine
            break;
        }

        if (StateReadX != "Centered" && StateReadY != "Centered") {
            Timer = 0;
        }

         else if (Timer > MaxTimer)
        {
            GetDraw.ClearLines();
            fizzles++;
            Timer = 0;

        }

    }



    public void OnRecognize(RecognitionResult result)
    {
        //List<string> SpellsCast = new List<string>();

        StopAllCoroutines();
        if (result != RecognitionResult.Empty)
        { //result is the ID for the recog
            CastSpell(result.gesture.id);

            if (lastspell != null)
            { 
                lastspell = result.gesture.id;
            }
            
        }

    }

    public void CastSpell(string id)
    {
        //If this gets too large, it can be put into seperate Spell Library script
        var draw = references.Where(e => e.pattern.id == id).FirstOrDefault();


        if (id == "vertical")
        {
            DownUsed++;
        }

        if(lastspell != string.Empty) { 
            SpellsCast.Add(lastspell);
        }

        recognized++;
        GetDraw.ClearLines();

        

    }

    public void MoveCalc() {


        CurrentX = Input.GetAxis("Mouse X");
        CurrentY = Input.GetAxis("Mouse Y");

        if (CurrentY < inDeadzoneY && CurrentY > -inDeadzoneY)
        {
            StateReadY = "Centered";
        }
        else if (CurrentY < -inDeadzoneY)
        {
            StateReadY = "Down";
        }
        else if (CurrentY > inDeadzoneY)
        {
            StateReadY = "Up";
        }

        if (CurrentX < inDeadzoneX && CurrentX > -inDeadzoneX)
        {
            StateReadX = "Centered";

        }
        else if (CurrentX < 0f - inDeadzoneX)
        {
            StateReadX = "Left";
        }
        else if (CurrentX > inDeadzoneX)
        {
            StateReadX = "Right";
        }
        MainPosText = StateReadX + "X/ " + StateReadY + "Y/";
    }
}
/*
1) See if lines are present
2) 
 */
