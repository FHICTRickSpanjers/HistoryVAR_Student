using UnityEngine;
using System.Collections.Generic;

//*************************************************************************************************************************************************
//SHARED CREATURES STUFF
//*************************************************************************************************************************************************
public class shared : MonoBehaviour 
{
	manager manager;
	[Header("RESOURCES")]
	public ParticleSystem blood;
	public ParticleSystem ripples;
	public ParticleSystem splash;
	Vector2 lastParticlePos=Vector2.zero;
	public Texture[] skin, eyes;
	public enum skinselect {SkinA, SkinB, SkinC};
	public enum eyesselect {Type0, Type1, Type2, Type3, Type4, Type5, Type6, Type7, Type8, Type9, Type10, Type11, Type12, Type13, Type14, Type15};
	[Space (10)]
	[Header("SETTINGS")]
	public skinselect BodySkin;
	public eyesselect EyesSkin;
	[Range(0.1f, 2.0f)] public float AnimSpeed=1.0f;
	public float Health=100;
	public float Water =100;
	public float Food =100;
	public float Fatigue =100;
	public float Damage =10;
	public float Armor =10;
	[Space (10)]
	[Header("ARTIFICIAL INTELLIGENCE")]
	public bool AI=false;
	public GameObject[] UserPath;
	[Range(0, 100)] public int PathPriority=50;

	[HideInInspector] public LODGroup lod;
	[HideInInspector] public Rigidbody body;
	[HideInInspector] public  Animator anm;
	[HideInInspector] public SkinnedMeshRenderer[] rend;
	[HideInInspector] public bool IsActive, IsVisible, IsDead, IsOnGround, IsOnWater, IsInWater, IsConstrained, CanAttack, CanFly, CanSwim, CanJump, IsJumping, IsAttacking, HasSideAttack, IsFlying;
	[HideInInspector] public float posY=0, currframe, lastframe, size, scale, delta, spineX_T, spineY_T, crouch_T, jaw_T;
	[HideInInspector] public string behavior, regime, specie;
	[HideInInspector] public Vector3 vectorTGT=Vector3.zero, FixedHeadPos=Vector3.zero;
	[HideInInspector] public GameObject objectTGT=null, objectCOL=null;
	float  terrainY=0, waterY=-65536, actioncount=0;
	int rndMove, rndIdle, loop;

//***********************************************************************************************************************************************************************************************************
//STARTUP VALUES
	void Awake()
	{
		manager=Camera.main.GetComponent<manager>();
		regime=transform.GetChild(0).tag;
		specie=transform.GetChild(0).name;
		lod=GetComponent<LODGroup>();
		anm=GetComponent<Animator>();
		body=GetComponent<Rigidbody>();
		rend=GetComponentsInChildren<SkinnedMeshRenderer>();
		SetScale(transform.localScale.x);
		SetMaterials(BodySkin.GetHashCode(), EyesSkin.GetHashCode());
		loop=Random.Range(0, 100);
		if(anm.parameters[0].name=="Attack") CanAttack=true;
		if(anm.parameters[1].name.Equals("Pitch")) CanFly=true;
		else if(anm.parameters[2].name.Equals("Pitch")) CanSwim=true;
		else if(anm.parameters[1].name.Equals("OnGround")) CanJump=true;
	}

//***********************************************************************************************************************************************************************************************************
//CHANGE SKIN
	#ifUNITY_EDITOR
	void OnDrawGizmosSelected()
	{
		if(rend[0].sharedMaterials[0].mainTexture!=skin[BodySkin.GetHashCode()] |
			rend[0].sharedMaterials[1].mainTexture!=eyes[EyesSkin.GetHashCode()])
		{ 
			foreach (SkinnedMeshRenderer element in rend)
			{
				Material a = element.sharedMaterial; element.sharedMaterial=new Material(a);
				element.sharedMaterials[0].mainTexture=skin[BodySkin.GetHashCode()];
				if(element.sharedMaterials.Length>1) element.sharedMaterials[1].mainTexture = eyes[EyesSkin.GetHashCode()];
			}
		}
	}
	#endif
	public void SetMaterials(int bodyindex, int eyesindex)
	{
		BodySkin= (skinselect) bodyindex; EyesSkin= (eyesselect) eyesindex;
		foreach (SkinnedMeshRenderer element in rend)
		{
			element.materials[0].mainTexture = skin[bodyindex];
			if(element.materials.Length>1) element.materials[1].mainTexture = eyes[eyesindex];
		}
	}
//***********************************************************************************************************************************************************************************************************
//ENABLE / DISABLE AI
	public void SetAI(bool UseAI) { AI=UseAI; if(!AI) { vectorTGT=Vector3.zero; objectTGT=null; objectCOL=null; } }
//***********************************************************************************************************************************************************************************************************
//SET SCALE
	public void SetScale(float Scale)
	{
		transform.localScale=new Vector3(Scale, Scale, Scale);
		size = (transform.GetChild(0).GetChild(0).position-transform.position).magnitude;
		scale = rend[0].bounds.extents.y;
	}
//***********************************************************************************************************************************************************************************************************
//COUNTERS
	void Update()
	{
		//Is creature currently visible or selected by manager ?
		if(rend[0].isVisible | rend[1].isVisible | rend[2].isVisible | transform.gameObject==manager.creaturesList[manager.creature].gameObject)
		{ IsVisible=true; IsActive=true; }
		else
		{ 
			IsVisible=false;
			//Realtime game ? If not, turn off all this creature activity
			if(!manager.RealtimeGame) { IsActive=false; return; }
		}

		//Get current animation frame
		if(currframe==15f | anm.GetAnimatorTransitionInfo(0).normalizedTime>0.5) { currframe=0.0f; lastframe=-1; }
		else currframe = Mathf.Round((anm.GetCurrentAnimatorStateInfo (0).normalizedTime % 1.0f) * 15f);

		//Manage health bar
		if(Health>0)
		{
			if(loop>=100)	
			{
				if((Water==0 | Food==0 | Fatigue==0)) Health-=0.1f; //decrease health
				else if(anm.GetInteger("Move")!=2) Health++; //increase health
				if(anm.GetInteger("Move")!=0) { Fatigue-=Random.Range(0.0f, 0.5f); if(!CanSwim) Water-=Random.Range(0.0f, 0.5f); else Water=100; Food-=Random.Range(0.0f, 0.5f); }//decrease needs
				loop=0;
			} else loop ++;
		}
		else
		{
			Water=0; Food=0; Fatigue=0;
			if(loop>=(10000*size))
			{
				//Delete from list and destroy gameobject
				if(manager.creature>=manager.creaturesList.IndexOf(transform.gameObject)) manager.creature--;
				manager.creaturesList.Remove(transform.gameObject); Destroy(transform.gameObject);
			} else loop ++;
		}
		//Clamp all parameters
		Health=Mathf.Clamp(Health, 0, 100); Water=Mathf.Clamp(Water, 0, 100); Food=Mathf.Clamp(Food, 0, 100); Fatigue=Mathf.Clamp(Fatigue, 0, 100);
	}

	///***********************************************************************************************************************************************************************************************************
	// KEYBOARD / MOUSE AND JOYSTICK INPUT (MANAGER ONLY) (allow to control all JP creatures)
	public void GetUserInputs(float yaw_max, float pitch_max, float crouch_max, float ang_max, float ang_t,
	                          int idle1=0, int idle2=0, int idle3=0, int idle4=0, int eat=0, int drink=0, int sleep=0, int rise=0)
	{
		// Current camera manager target ?
		if(manager.UseManager && transform.gameObject==manager.creaturesList[manager.creature].gameObject && manager.CameraMode!=0)
		{
			if(actioncount==0) { objectTGT=null; actioncount=2500; behavior="Player"; } else actioncount--;
			
			//Run
			float run; if(!Input.GetKey(KeyCode.LeftShift) && (Input.GetKey(KeyCode.W) | Input.GetKey(KeyCode.S) | Input.GetKey(KeyCode.A) | Input.GetKey(KeyCode.D)))
				run= 3.0f; else run=0.95f;
			//Attack
			if(CanAttack) { if(Input.GetKey(KeyCode.Mouse0)) anm.SetBool ("Attack", true); else anm.SetBool ("Attack", false); }
			//Crouch (require "JP script extension" asset)
			if(IsOnGround && Input.GetKey(KeyCode.LeftControl)) crouch_T = crouch_max*transform.localScale.x;
			//Fly Up/Down
			if(CanFly | CanSwim)
			{
				if(Input.GetKey(KeyCode.Mouse1))
				{
					if(Input.GetAxis("Mouse X")!=0)//Turn 
					anm.SetFloat("Turn", Mathf.LerpAngle(anm.GetFloat("Turn"), Input.GetAxis("Mouse X")*ang_max, ang_t));
					else anm.SetFloat("Turn", Mathf.LerpAngle(anm.GetFloat("Turn"), 0, ang_t));
					if(Input.GetAxis("Mouse Y")!=0) //Pitch 
					anm.SetFloat("Pitch", Mathf.LerpAngle(anm.GetFloat("Pitch"), Input.GetAxis("Mouse Y")*ang_max*2, ang_t));
					else anm.SetFloat("Pitch", Mathf.LerpAngle(anm.GetFloat("Pitch"), 0, ang_t));
				}
				else
				{
					if(Input.GetKey(KeyCode.LeftControl)) anm.SetFloat ("Pitch", 1.0f);
					else if(Input.GetKey(KeyCode.Space)) anm.SetFloat ("Pitch", -1.0f);
					else anm.SetFloat ("Pitch", 0.0f);
				}
			}
			//Jump
			if(CanJump && IsOnGround && Input.GetKey(KeyCode.Space)) anm.SetInteger ("Move", 3);
			//Moving
			else if(Input.GetAxis("Horizontal")!=0 | Input.GetAxis("Vertical")!=0)
			{
				if(CanSwim) //Swim type
				{
					if(Mathf.Abs(Input.GetAxis("Horizontal"))+Mathf.Abs(Input.GetAxis("Vertical")) <run) anm.SetInteger ("Move", 1); //Walk
					else  anm.SetInteger ("Move", 2); //Run
					delta = Mathf.DeltaAngle(manager.transform.eulerAngles.y, transform.eulerAngles.y-Mathf.Atan2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"))*Mathf.Rad2Deg);
					anm.SetFloat("Turn", Mathf.LerpAngle(anm.GetFloat("Turn"), Mathf.Clamp(-delta/45, -ang_max, ang_max), ang_t)); //Turn
				}
				else if(CanFly&&!IsOnGround) //Flying type
				{
					if(Input.GetKey(KeyCode.Mouse1))
					{
						if(Input.GetAxis("Vertical")<0) anm.SetInteger ("Move", -1); //Backward
						else if(Input.GetAxis("Vertical")>0) anm.SetInteger ("Move", 3); //Forward
						else if(Input.GetAxis("Horizontal")>0) anm.SetInteger ("Move", -10); //Strafe-
						else if(Input.GetAxis("Horizontal")<0) anm.SetInteger ("Move", 10); //Strafe+
						else anm.SetInteger ("Move", 0);
					}
					else
					{
						if(Mathf.Abs(Input.GetAxis("Horizontal"))+Mathf.Abs(Input.GetAxis("Vertical")) <run) anm.SetInteger ("Move", 2); //Glide
						else anm.SetInteger ("Move", 1); //Fly forward
						
						delta = Mathf.DeltaAngle(manager.transform.eulerAngles.y, transform.eulerAngles.y-Mathf.Atan2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"))*Mathf.Rad2Deg);
						anm.SetFloat("Turn", Mathf.LerpAngle(anm.GetFloat("Turn"), Mathf.Clamp(-delta/15, -ang_max, ang_max), ang_t)); //Turn
					}
				}
				else if(HasSideAttack) // Tail attack type
				{
					if(Input.GetAxis("Vertical")<0 && Input.GetAxis("Vertical")<run) anm.SetInteger ("Move", 1); //Forward
					else if(Input.GetAxis("Vertical")>0) anm.SetInteger ("Move", 2); //Run
					else if(Input.GetAxis("Horizontal")>0) anm.SetInteger ("Move", -10); //Strafe-
					else if(Input.GetAxis("Horizontal")<0) anm.SetInteger ("Move", 10); //Strafe+
				}
				else //Default type
				{
					if(Input.GetKey(KeyCode.Mouse1))
					{
						if(Input.GetAxis("Vertical")>0 && Input.GetAxis("Vertical")<run) anm.SetInteger ("Move", 1); //Forward
						else if(Input.GetAxis("Vertical")>0) anm.SetInteger ("Move", 2); //Run
						else if(Input.GetAxis("Vertical")<0) anm.SetInteger ("Move", -1);	//Backward
						else if(Input.GetAxis("Horizontal")>0) anm.SetInteger ("Move", -10); //Strafe-
						else if(Input.GetAxis("Horizontal")<0) anm.SetInteger ("Move", 10); //Strafe+
						anm.SetFloat("Turn", Mathf.LerpAngle(anm.GetFloat("Turn"), Input.GetAxis("Mouse X")*ang_max, ang_t)); //Turn
					}
					else
					{
						delta = Mathf.DeltaAngle(manager.transform.eulerAngles.y, transform.eulerAngles.y-Mathf.Atan2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"))*Mathf.Rad2Deg);
						if(Mathf.Abs(Input.GetAxis("Horizontal"))+Mathf.Abs(Input.GetAxis("Vertical")) <run)
						{
							if(delta>135) anm.SetInteger ("Move", -10); //Turn
							else if(delta<-135) anm.SetInteger ("Move", 10); //Turn
							else anm.SetInteger ("Move", 1); //Walk
						}
						else
						{
							if(delta>135 | delta<-135) anm.SetInteger ("Move", 1); //Walk
							else anm.SetInteger ("Move", 2); //Run
						}
						anm.SetFloat("Turn", Mathf.LerpAngle(anm.GetFloat("Turn"), Mathf.Clamp(-delta/45, -ang_max, ang_max), ang_t)); //Turn
					}
				}
				
				//(require "JP script extension" asset)
				if(IsOnGround && !IsFlying && !IsAttacking && Input.GetKey(KeyCode.LeftControl)) anm.speed=AnimSpeed/3.0f; //Crouch speed modifer
				else { crouch_T=0; anm.speed = AnimSpeed; }//Default speed
			}
			//Stoped
			else
			{
				if(CanSwim) //Swim type
				{
					if(anm.GetFloat("Pitch")!=0)
					{
						if(Input.GetKey(KeyCode.LeftShift)) anm.SetInteger ("Move", 2); else anm.SetInteger ("Move", 1);
						anm.SetFloat("Turn", Mathf.LerpAngle(anm.GetFloat("Turn"), 0, ang_t));
					} else { anm.SetInteger ("Move", 0); anm.SetFloat("Turn", Mathf.LerpAngle(anm.GetFloat("Turn"), 0, ang_t)); } //Stop
				}
				else if(CanFly && !IsOnGround) //Flying type
				{ anm.SetInteger ("Move", 0); anm.SetFloat("Turn", Mathf.LerpAngle(anm.GetFloat("Turn"), 0, ang_t)); } //Stationary
				else if(HasSideAttack) //Tail attack type
				{
					delta = Mathf.DeltaAngle(manager.transform.eulerAngles.y, transform.eulerAngles.y);
					if(delta>-135 && delta<0 && anm.GetBool("Attack")) anm.SetInteger ("Move", 10);
					else if(delta<135 && delta>0 && anm.GetBool("Attack")) anm.SetInteger ("Move", -10); 
					else anm.SetInteger ("Move", 0);
				}
				else //Default type
				{
					if(Input.GetKey(KeyCode.Mouse1))
					{
						if(Input.GetAxis("Mouse X")>0) anm.SetInteger ("Move", -10); //Strafe- 
						else if(Input.GetAxis("Mouse X")<0) anm.SetInteger ("Move", 10); //Strafe+
						else anm.SetInteger ("Move", 0);
						anm.SetFloat("Turn", Mathf.LerpAngle(anm.GetFloat("Turn"), Mathf.Clamp(ang_max*Input.GetAxis("Mouse X"), -ang_max, ang_max), ang_t));
					}
					else { anm.SetInteger ("Move", 0); anm.SetFloat("Turn", Mathf.LerpAngle(anm.GetFloat("Turn"), 0, ang_t)); } //Stop
				}
				
				if(!Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.Space)) crouch_T=Mathf.PingPong(Time.time/4,(crouch_max/2))*transform.localScale.x; //(require "JP script extension" asset)
				anm.speed=AnimSpeed; //Default speed
			}
			
			//Look direction
			if(Input.GetKey(KeyCode.Mouse2) && Cursor.lockState==CursorLockMode.Locked)
			{
				spineX_T+=Input.GetAxis("Mouse X"); spineX_T=Mathf.Clamp(spineX_T, -yaw_max, yaw_max);
				spineY_T+=Input.GetAxis("Mouse Y"); spineY_T=Mathf.Clamp(spineY_T, -pitch_max, pitch_max);
			}
			else if(Cursor.lockState==CursorLockMode.Locked && AnimSpeed>0)
			{
				spineX_T=yaw_max*-(Mathf.DeltaAngle(manager.transform.eulerAngles.y, transform.eulerAngles.y)/(180-yaw_max));
				spineY_T=pitch_max*(Mathf.DeltaAngle(manager.transform.eulerAngles.x, transform.eulerAngles.x)/(90+pitch_max));
			}
			
			//Idles
			if(Input.GetKey(KeyCode.E))
			{
				if(Input.GetKeyDown(KeyCode.E))
				{
					int idles_lenght=0; if(idle1>0) idles_lenght++; if(idle2>0) idles_lenght++; if(idle3>0) idles_lenght++; if(idle4>0) idles_lenght++; //idles to play
					rndIdle = Random.Range(1, idles_lenght+1);
				}
				if(rndIdle==1) anm.SetInteger ("Idle", idle1);
				else if(rndIdle==2) anm.SetInteger ("Idle", idle2);
				else if(rndIdle==3) anm.SetInteger ("Idle", idle3);
				else if(rndIdle==4) anm.SetInteger ("Idle", idle4);
			}
			else if(Input.GetKey(KeyCode.F)) //Eat / Drink
			{
				if(vectorTGT==Vector3.zero) FindPlayerFood(); //looking for food
				//Drink
				if(vectorTGT==Vector3.zero && IsOnWater) { anm.SetInteger ("Idle", drink); if(Water<100) { behavior="Drink"; Water+=0.05f; } }
				//Eat
				else if(vectorTGT!=Vector3.zero) { anm.SetInteger ("Idle", eat); if(Food<100) { behavior="Eat"; Food+=0.05f; } if(Input.GetKeyUp(KeyCode.F)) vectorTGT=Vector3.zero; }
				//nothing found
				else manager.message=1;
			}
			else if(Input.GetKey(KeyCode.Q)) { anm.SetInteger("Idle", sleep); if(anm.GetInteger("Move")!=0) anm.SetInteger ("Idle", 0); } //Sleep/Sit
			else if(rise!=0 && Input.GetKey(KeyCode.Space)) anm.SetInteger ("Idle", rise); //Rise
			else { anm.SetInteger ("Idle", 0); vectorTGT=Vector3.zero; }
			
			if(anm.GetCurrentAnimatorStateInfo(0).IsName(specie+"|Sleep") && Fatigue<100) Fatigue+=0.05f; 
		}
		// Not current camera target, reset parameters
		else if(AnimSpeed>0)
		{
			if(CanAttack) anm.SetBool ("Attack", false);
			if(CanFly) anm.SetFloat ("Pitch", 0.0f); anm.SetFloat ("Turn", 0.0f);
			anm.SetInteger ("Move", 0); anm.SetInteger ("Idle", 0);
			spineX_T=0; spineY_T=0; crouch_T=0;
		}
	}

//***********************************************************************************************************************************************************************************************************
// FIND PLAYER FOOD
bool FindPlayerFood()
{
		//Find carnivorous food (looking for a dead creature)
		if(regime.Equals("Carnivorous"))
		{
			foreach (GameObject element in manager.creaturesList.ToArray())
			{
				if((element.transform.position-transform.position).magnitude>(scale*2)) continue; //not in range
				shared otherCreature= element.GetComponent<shared>(); //Get other creature script
				if(otherCreature.IsDead) { objectTGT=otherCreature.gameObject; vectorTGT = otherCreature.transform.position; return true; } // meat found
			}
		}
		else
		{
			//Find herbivorous food (looking for trees/details on terrain )
			if(manager.terrain)
			{
				//Large creature, look for trees
				if(size>8) 
				{
					Vector3 V1=Vector3.zero;  float i=0; RaycastHit hit;
					while(i<360)
					{
						V1=transform.position+(Quaternion.Euler(0, i, 0)*Vector3.forward*(scale*2));
						if(Physics.Linecast(V1+Vector3.up*size, transform.position+Vector3.up*size, out hit, manager.treeLayer))
						{ vectorTGT = hit.point; return true; } //tree found
						else { i++; V1=Vector3.zero; } // not found, continue
					}
				}
				//Look for grass detail
				else
				{
					TerrainData data=manager.terrain.terrainData;
					int res= data.detailResolution, layer=0;
					float x = ((transform.position.x - manager.terrain.transform.position.x) / data.size.z * res), y = ((transform.position.z - manager.terrain.transform.position.z) / data.size.x * res);

					for(layer=0; layer<data.detailPrototypes.Length; layer++)
					{
						if(data.GetDetailLayer( (int) x,  (int) y, 1, 1, layer) [ 0, 0]>0)
						{
							vectorTGT.x=(data.size.x/res)*x+manager.terrain.transform.position.x;
							vectorTGT.z=(data.size.z/res)*y+manager.terrain.transform.position.z;
							vectorTGT.y = manager.terrain.SampleHeight( new Vector3(vectorTGT.x, 0, vectorTGT.z)); 
							objectTGT=null; return true; 
						}
					}
				}
			}
		}

		objectTGT=null; vectorTGT=Vector3.zero; return false; //nothing found...
}

//***********************************************************************************************************************************************************************************************************
// MANAGE COLLISIONS // PARTICLES FX
	public void ManageCollision(Collision col, float pitch_max, float crouch_max, AudioSource[] source, AudioClip pain, AudioClip Hit_jaw, AudioClip Hit_head, AudioClip Hit_tail)
	{
		// Is a creature
		if(col.gameObject.tag.Equals("Creature"))
		{
			shared otherCreature =col.gameObject.GetComponent<shared>(); //Get other creature script
			float myScale=transform.localScale.x, otherScale=otherCreature.transform.localScale.x; //get scales

			//Creature attack
			if(!col.collider.gameObject.name.Equals("root") && otherCreature.IsAttacking)
			{
				if(col.collider.gameObject.name.Equals("jaw0")) //Get bite damage
				{
					if(regime.Equals("Carnivorous")) Health-= (otherCreature.Damage*otherScale) / (Armor*myScale); // Carnivorous
					else Health-= ((otherCreature.Damage*otherScale)*2) / (Armor*myScale); //Herbivorous
				}
				else //Get head/tail/feet damage
				{
					body.AddExplosionForce(250, col.contacts[0].point, size);
					if(regime.Equals("Carnivorous")) Health-= ((otherCreature.Damage*otherScale)*2) / (Armor*myScale); // Carnivorous
					else Health-= ( (otherCreature.Damage*otherScale)/2) / (Armor*myScale); //Herbivorous
				}

				if(jaw_T==0)
				{
					ParticleSystem particle=null;
					particle = Instantiate(blood, col.contacts[0].point, Quaternion.Euler(-90, 0, 0))as ParticleSystem; //Spawn blood particle
					DestroyObject(particle.gameObject, 1.0f); //Destroy blood particle
					crouch_T=crouch_max/2; spineY_T =pitch_max/2; jaw_T=32; //Animate
					source[0].pitch=Random.Range(1.0f, 1.5f); 
					if(Random.Range(0, 3)==0) source[0].PlayOneShot(pain, 2.0f); //pain sound
					if(col.collider.gameObject.name.Equals("jaw0")) source[1].PlayOneShot(Hit_jaw, 1.0f); //hit by jaw
					else if(col.collider.gameObject.name.Equals("head")) source[1].PlayOneShot(Hit_head, 1.0f); //hit by head
					else source[1].PlayOneShot(Hit_tail, 1.0f); //hit by tail
				}
				//Othercreature are a player
				if(!otherCreature.AI)
				{
					otherCreature.objectTGT=transform.gameObject; otherCreature.actioncount=2500;
					if(otherCreature.regime.Equals("Carnivorous")) otherCreature.behavior="Hunt";
					else otherCreature.behavior="Defend";
				}
			}
		}
		else if(col.contacts[0].point.y>(transform.position.y+size/4) && loop==0) objectCOL=col.gameObject;
		else objectCOL=null;
	}

	//Spawn water particle FX
	void WaterParticleFX(Collider col, ParticleSystem particleFx)
	{
		ParticleSystem particle=null;
		if(col.gameObject.layer==4)
		{
			waterY=col.transform.position.y; //Get current water layer altitude, (multiple waterbody supported)
			if(body.velocity.magnitude > 0.05f)
			{
				if((!IsFlying && !IsJumping && !IsAttacking) | CanSwim)
				{
					if((particleFx==splash && (IsOnGround | IsInWater)) | (particleFx==ripples && !IsOnWater)) return;
				}

				Vector2 Pos=new Vector2(body.worldCenterOfMass.x, body.worldCenterOfMass.z);
				float dist=(lastParticlePos-Pos).magnitude, particleSize=Mathf.Clamp(size, 0.5f, 4f*transform.localScale.x);
				if(dist>1.0f | particleFx==splash)
			 	{
					particle=Instantiate(particleFx, new Vector3(Pos.x, waterY+0.01f, Pos.y), Quaternion.Euler(-90, 0, 0)) as ParticleSystem;
					lastParticlePos=new Vector2(Pos.x, Pos.y); particle.transform.localScale=new Vector3(particleSize, particleSize, particleSize);
					DestroyObject(particle.gameObject, 3.0f);
				}
			}
		} else waterY=-65536;
	}

	void OnTriggerEnter(Collider col) { WaterParticleFX(col, splash); }
	void OnTriggerStay(Collider col) { WaterParticleFX(col, ripples); }
	void OnTriggerExit(Collider col) { WaterParticleFX(col, splash); }


//***********************************************************************************************************************************************************************************************************
//GET GROUND / WATER ALTITUDE (get Terrain collider or walkable/water layer altitude and normal, return y position)

	public void GetGroundAlt(bool quadruped, float crouch)
	{
		Vector3 normal=Vector3.zero; RaycastHit hit;
		//Use raycast, can walk on any kind of collider with "walkable'' layer
		if(manager.UseRaycast) 
		{
			if(Physics.Raycast(transform.position+Vector3.up*size, -Vector3.up, out hit, size*2, manager.walkableLayer))
			{ terrainY = hit.point.y; normal=hit.normal; }  else terrainY=-65536;
		}
		// Unity "Terrain collider" only
		else
		{
			terrainY=manager.terrain.SampleHeight(transform.position)+manager.terrain.GetPosition().y;
			float res=manager.terrain.terrainData.heightmapResolution;
			float x = ((transform.position.x -manager.terrain.transform.position.x) / manager.terrain.terrainData.size.x ) * res;
			float y = ((transform.position.z - manager.terrain.transform.position.z) / manager.terrain.terrainData.size.z ) * res;
			if(x>res | x<0 | y>res | y<0) posY=transform.position.y;
			else normal=manager.terrain.terrainData.GetInterpolatedNormal(x/res, y/res);
		}

		//Is in water
		if(transform.position.y<waterY)
		{
			if((transform.position.y-waterY)>-body.centerOfMass.y-transform.localScale.x) { IsOnWater=true; IsInWater=false; } //On water
			else { IsOnWater=false; IsInWater=true; } //Underwater
		}
		//Not In/on water
		else { IsOnWater=false; IsInWater=false; }

		//Not on ground
		if((transform.position.y-terrainY)>transform.localScale.x)
		{
			IsOnGround=false; if(transform.position.y<waterY) IsInWater=true; else IsInWater=false;
			if(IsConstrained) body.detectCollisions=false; else body.detectCollisions=true;
			transform.rotation=Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, transform.eulerAngles.y, 0), 0.02f);
		}
		//On ground
		else
		{
			if(IsConstrained | IsFlying)
			{
				IsOnGround=true;
				if(IsFlying) body.detectCollisions=true; else body.detectCollisions=false;
				transform.rotation=Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Vector3.Cross(transform.right, normal), normal), 0.02f);
			}
			else
			{
				IsOnGround=true;
				body.detectCollisions=true;
				if(normal!=Vector3.zero)
				{
					Quaternion Normal=Quaternion.LookRotation(Vector3.Cross(transform.right, normal), normal);
					float pitch = Mathf.Clamp(Mathf.DeltaAngle(Normal.eulerAngles.x, 0.0f), -20, 20), roll = Mathf.Clamp(Mathf.DeltaAngle(Normal.eulerAngles.z, 0.0f), -8, 8);
					transform.rotation=Quaternion.Lerp(transform.rotation, Quaternion.Euler(-pitch, transform.eulerAngles.y, -roll), 0.02f);
				}
			}
		}

		if(IsInWater && !IsDead) posY=waterY-body.centerOfMass.y;
		else if(IsOnGround) posY=terrainY;
		else
		{
			posY=transform.position.y;
			transform.rotation=Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, transform.eulerAngles.y, 0), 0.02f);
		}
	}


//***********************************************************************************************************************************************************************************************************
//
// FEET IINVERSE KINEMATICS Require "JP Script Extension Asset"
//
//***********************************************************************************************************************************************************************************************************
//***********************************************************************************************************************************************************************************************************
//QUADRUPED
	public void QuadIK(Transform RArm1, Transform RArm2, Transform RArm3, Transform LArm1, Transform LArm2, Transform LArm3,
										Transform RLeg1, Transform RLeg2, Transform RLeg3, Transform LLeg1, Transform LLeg2, Transform LLeg3)
	{
		if(manager.UseIK) manager.message=2; manager.UseIK=false; return;
	}
//***********************************************************************************************************************************************************************************************************
//SMALL BIPED
	public void SmallBipedIK(Transform RLeg1, Transform RLeg2, Transform RLeg3, Transform RLeg4,
													Transform LLeg1, Transform LLeg2, Transform LLeg3, Transform LLeg4)
	{ if(manager.UseIK) manager.message=2; manager.UseIK=false; return; }
//***********************************************************************************************************************************************************************************************************
//LARGE BIPED
	public void LargeBipedIK(Transform RLeg1, Transform RLeg2, Transform RLeg3, Transform RLeg4,
												Transform LLeg1, Transform LLeg2, Transform LLeg3, Transform LLeg4)
	{ if(manager.UseIK) manager.message=2; manager.UseIK=false; return; }
//***********************************************************************************************************************************************************************************************************
//CONVEX QUADRUPED
	public void ConvexQuadIK(Transform RArm1, Transform RArm2, Transform RArm3, Transform LArm1, Transform LArm2, Transform LArm3,
													Transform RLeg1, Transform RLeg2, Transform RLeg3,Transform LLeg1, Transform LLeg2, Transform LLeg3)
	{ if(manager.UseIK) manager.message=2; manager.UseIK=false; return; }
//***********************************************************************************************************************************************************************************************************
//FLYING
	public void FlyingIK(Transform RArm1, Transform RArm2, Transform RArm3, Transform LArm1, Transform LArm2, Transform LArm3,
										Transform RLeg1, Transform RLeg2, Transform RLeg3, Transform LLeg1, Transform LLeg2, Transform LLeg3)
	{ if(manager.UseIK) manager.message=2; manager.UseIK=false; return; }

//***********************************************************************************************************************************************************************************************************
//
// ARTIFICIAL INTELLIGENCERequire "JP Script Extension Asset"
//
//***********************************************************************************************************************************************************************************************************
	//***********************************************************************************************************************************************************************************************************
	//BASE AI (AI entry point for all JP creatures)
	public void BaseAI(Vector3 HeadPos, float yaw_max, float pitch_max, float crouch_max, float ang_max, float ang_t, int idle1=0, int idle2=0, int idle3=0, int idle4=0, int eat=0, int drink=0, int sleep=0)
	{ if(AI) manager.message=2; AI=false; return; }

}


