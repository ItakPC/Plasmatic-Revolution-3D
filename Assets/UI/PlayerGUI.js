#pragma strict

var size : Vector2 = new Vector2(48, 48);

var healthPos : Vector2 = new Vector2(20, 200);
var healthBarDisplay : float = 1;
var healthBarEmpty : Texture2D;
var healthBarFull : Texture2D;

var hungerPos : Vector2 = new Vector2(20, 60);
var hungerBarDisplay : float = 1;
var hungerBarEmpty : Texture2D;
var hungerBarFull : Texture2D;

var healthFallRate : int = 150;
var hungerFallRate : int = 150;

private var chMotor : CharacterMotor;
private var controller : CharacterController;

var canJump : boolean = false;

var jumpTimer : float = 0.7;

function Start()
{
	chMotor = GetComponent(CharacterMotor);
	controller = GetComponent(CharacterController);
}

function OnGUI()
{

	GUI.BeginGroup(new Rect (healthPos.x, healthPos.y, size.x, size.y));
	GUI.Box(Rect(0, 0, size.x, size.y), healthBarEmpty);
	
	GUI.BeginGroup(new Rect (0, 0, size.x * healthBarDisplay, size.y));
	GUI.Box(Rect(0, 0, size.x, size.y), healthBarFull);
	
	GUI.EndGroup();
	GUI.EndGroup();

	GUI.BeginGroup(new Rect (hungerPos.x, hungerPos.y, size.x, size.y));
	GUI.Box(Rect(0, 0, size.x, size.y), hungerBarEmpty);
	
	GUI.BeginGroup(new Rect (0, 0, size.x * hungerBarDisplay, size.y));
	GUI.Box(Rect(0, 0, size.x, size.y), hungerBarFull);
	
	GUI.EndGroup();
	GUI.EndGroup();
}

function Update()
{
	
	//TODO: Rewrite Hunger and Health Code	
	
}

function Wait()
{
	yield WaitForSeconds(0.1);
	canJump = false;
}





















































































