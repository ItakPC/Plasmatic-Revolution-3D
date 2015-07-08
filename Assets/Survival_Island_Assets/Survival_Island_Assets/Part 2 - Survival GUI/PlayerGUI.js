#pragma strict

var size : Vector2 = new Vector2(64, 64);

var healthPos : Vector2 = new Vector2(20, 200);
var healthBarDisplay : float = 1;
var healthBarEmpty : Texture2D;
var healthBarFull : Texture2D;

var hungerPos : Vector2 = new Vector2(20, 60);
var hungerBarDisplay : float = 1;
var hungerBarEmpty : Texture2D;
var hungerBarFull : Texture2D;

var thirstPos : Vector2 = new Vector2(20, 200);
var thirstBarDisplay : float = 1;
var thirstBarEmpty : Texture2D;
var thirstBarFull : Texture2D;

var staminaPos : Vector2 = new Vector2(20, 200);
var staminaBarDisplay : float = 1;
var staminaBarEmpty : Texture2D;
var staminaBarFull : Texture2D;

var healthFallRate : int = 150;
var hungerFallRate : int = 150;
var thirstFallRate : int = 100;
var staminaFallRate : int = 35;

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

	GUI.BeginGroup(new Rect (thirstPos.x, thirstPos.y, size.x, size.y));
	GUI.Box(Rect(0, 0, size.x, size.y), thirstBarEmpty);
	
	GUI.BeginGroup(new Rect (0, 0, size.x * thirstBarDisplay, size.y));
	GUI.Box(Rect(0, 0, size.x, size.y), thirstBarFull);
	
	GUI.EndGroup();
	GUI.EndGroup();
	
	GUI.BeginGroup(new Rect (staminaPos.x, staminaPos.y, size.x, size.y));
	GUI.Box(Rect(0, 0, size.x, size.y), staminaBarEmpty);
	
	GUI.BeginGroup(new Rect (0, 0, size.x * staminaBarDisplay, size.y));
	GUI.Box(Rect(0, 0, size.x, size.y), staminaBarFull);
	
	GUI.EndGroup();
	GUI.EndGroup();
}

function Update()
{

	if(hungerBarDisplay <= 0 && (thirstBarDisplay <= 0))
	{
		healthBarDisplay -= Time.deltaTime / healthFallRate * 2;
	}
	
	else
	{
		if(hungerBarDisplay <= 0 || thirstBarDisplay <= 0)
		{
			healthBarDisplay -= Time.deltaTime / healthFallRate;
		}
	}
	
	if(healthBarDisplay <= 0)
	{
	
	}

	if(hungerBarDisplay >= 0)
	{
		hungerBarDisplay -= Time.deltaTime / hungerFallRate;
	}
	
	if(hungerBarDisplay <= 0)
	{
		hungerBarDisplay = 0;
	}
	
	if(hungerBarDisplay >= 1)
	{
		hungerBarDisplay = 1;
	}
	
	if(thirstBarDisplay >= 0)
	{
		thirstBarDisplay -= Time.deltaTime / thirstFallRate;
	}
	
	if(thirstBarDisplay <= 0)
	{
		thirstBarDisplay = 0;
	}
	
	if(thirstBarDisplay >= 1)
	{
		thirstBarDisplay = 1;
	}
	
	if(controller.velocity.magnitude > 0 && Input.GetKey(KeyCode.LeftShift))
	{
		chMotor.movement.maxForwardSpeed = 10;
		chMotor.movement.maxSidewaysSpeed = 10;
		staminaBarDisplay -= Time.deltaTime / staminaFallRate;
	}
	
	else
	{
		chMotor.movement.maxForwardSpeed = 6;
		chMotor.movement.maxSidewaysSpeed = 6;
		staminaBarDisplay += Time.deltaTime / staminaFallRate;
	}
	
	if(chMotor.jumping.jumping == true && canJump == true)
	{
		staminaBarDisplay -= (0.2 * 0.14) / 2.85;
		Wait();
	}
	
	if(canJump == false)
	{
		jumpTimer -= Time.deltaTime;
		chMotor.jumping.enabled = false;
	}
	
	if(jumpTimer <= 0 && staminaBarDisplay >= 0.14)
	{
		canJump = true;
		chMotor.jumping.enabled = true;
		jumpTimer = 0.7;
	}
	
	if(staminaBarDisplay >= 1)
	{
		staminaBarDisplay = 1;
	}
	
	if(staminaBarDisplay <= 0)
	{
		staminaBarDisplay = 0;
		canJump = false;
		chMotor.jumping.enabled = false;
		chMotor.movement.maxForwardSpeed = 6;
		chMotor.movement.maxSidewaysSpeed = 6;
	}
}

function Wait()
{
	yield WaitForSeconds(0.1);
	canJump = false;
}





















































































