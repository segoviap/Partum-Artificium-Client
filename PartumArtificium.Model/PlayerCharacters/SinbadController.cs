using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;

namespace PartumArtificium.Model.PlayerCharacters
{
    /// <summary> </summary>
	public class SinbadController : CharacterController
	{
		#region Constants
		private const int NumberOfAnimations = 13;
		private const int CharacterHeight = 5;
		private const float CameraHeight = 2;
		private const int RunSpeed = 17;
		private const float TurnSpeed = 500.0f;
		private const float AnitmationFadeSpeed = 7.5f;
		private const float JumpAcceleration = 30.0f;
		private const float Gravity = 90.0f;
		#endregion

		//Camera
		private SceneNode _cameraNode;
		private SceneNode _cameraPivot;
		private SceneNode _cameraGoal;
		private float _pivotPitch = 0f;

		//Model variables
		private SceneNode _bodyNode;
		private Entity _bodyEntity;
		private Entity _sword1;
		private Entity _sword2;

		//Animation variables
		private SinbadAnimationID _currentBaseAnimationId = SinbadAnimationID.ANIM_NONE;
		private SinbadAnimationID _currentTopAnimationId = SinbadAnimationID.ANIM_NONE;
		private AnimationState[] _animations;
		private bool[] _fadingIn;
		private bool[] _fadingOut;
		private bool _swordsDrawn = false;

		private float _verticalVelocity = 0f;
		private Vector3 _keyDirection = Vector3.ZERO;

        private Vector3 _goalDirection = Vector3.ZERO;
        private float _timer = 0f;

		private enum SinbadAnimationID
		{
			ANIM_IDLE_BASE,
			ANIM_IDLE_TOP,
			ANIM_RUN_BASE,
			ANIM_RUN_TOP,
			ANIM_HANDS_CLOSED,
			ANIM_HANDS_RELAXED,
			ANIM_DRAW_SWORDS,
			ANIM_SLICE_VERTICAL,
			ANIM_SLICE_HORIZONTAL,
			ANIM_DANCE,
			ANIM_JUMP_START,
			ANIM_JUMP_LOOP,
			ANIM_JUMP_END,
			ANIM_NONE
		}

		public SinbadController(Camera camera) 
			: base(camera)
		{
            SetupAnimations();
            SetupCamera(camera);
        }
        #region Public Methods
        /// <summary> </summary>
        /// <param name="deltaTime"></param>
        public void AddTime(float deltaTime)
        {
            UpdateBody(deltaTime);
            UpdateAnimations(deltaTime);
            UpdateCamera(deltaTime);
        }

		public void InjectKeyDown(MOIS.KeyEvent e)
	    {
		    if (e.key == MOIS.KeyCode.KC_Q && (_currentTopAnimationId == SinbadAnimationID.ANIM_IDLE_TOP || _currentTopAnimationId == SinbadAnimationID.ANIM_RUN_TOP))
		    {
			    // take swords out (or put them back, since it's the same animation but reversed)
			    SetTopAnimation(SinbadAnimationID.ANIM_DRAW_SWORDS, true);
			    _timer = 0;
		    }
		    else if (e.key == MOIS.KeyCode.KC_E && !_swordsDrawn)
		    {
			    if (_currentTopAnimationId == SinbadAnimationID.ANIM_IDLE_TOP || _currentTopAnimationId == SinbadAnimationID.ANIM_RUN_TOP)
			    {
				    // start dancing
				    SetBaseAnimation(SinbadAnimationID.ANIM_DANCE, true);
				    SetTopAnimation(SinbadAnimationID.ANIM_NONE, false);
				    // disable hand animation because the dance controls hands
				    _animations[(int)SinbadAnimationID.ANIM_HANDS_RELAXED].Enabled = false;
				}
				else if (_currentBaseAnimationId == SinbadAnimationID.ANIM_DANCE)
				{
					// stop dancing
					SetBaseAnimation(SinbadAnimationID.ANIM_IDLE_BASE, false);
					SetTopAnimation(SinbadAnimationID.ANIM_IDLE_TOP, false);
					// re-enable hand animation
					_animations[(int)SinbadAnimationID.ANIM_HANDS_RELAXED].Enabled = true;
				}
			}

			// keep track of the player's intended direction
			else if (e.key == MOIS.KeyCode.KC_W) _keyDirection.z = -1;
			else if (e.key == MOIS.KeyCode.KC_A) _keyDirection.x = -1;
			else if (e.key == MOIS.KeyCode.KC_S) _keyDirection.z = 1;
			else if (e.key == MOIS.KeyCode.KC_D) _keyDirection.x = 1;

			else if (e.key == MOIS.KeyCode.KC_SPACE && (_currentTopAnimationId == SinbadAnimationID.ANIM_IDLE_TOP || _currentTopAnimationId == SinbadAnimationID.ANIM_RUN_TOP))
			{
				// jump if on ground
				SetBaseAnimation(SinbadAnimationID.ANIM_JUMP_START, true);
				SetTopAnimation(SinbadAnimationID.ANIM_NONE, false);
				_timer = 0;
			}

			if (!_keyDirection.IsZeroLength && _currentBaseAnimationId == SinbadAnimationID.ANIM_IDLE_BASE)
			{
				// start running if not already moving and the player wants to move
				SetBaseAnimation(SinbadAnimationID.ANIM_RUN_BASE, true);
				if (_currentTopAnimationId == SinbadAnimationID.ANIM_IDLE_TOP) 
				{
					SetTopAnimation(SinbadAnimationID.ANIM_RUN_TOP, true);
				}
			}
		}

		public void InjectKeyUp(MOIS.KeyEvent e)
		{
			// keep track of the player's intended direction
			if (e.key == MOIS.KeyCode.KC_W && _keyDirection.z == -1) _keyDirection.z = 0;
			else if (e.key == MOIS.KeyCode.KC_A && _keyDirection.x == -1) _keyDirection.x = 0;
			else if (e.key == MOIS.KeyCode.KC_S && _keyDirection.z == 1) _keyDirection.z = 0;
			else if (e.key == MOIS.KeyCode.KC_D && _keyDirection.x == 1) _keyDirection.x = 0;

			if (_keyDirection.IsZeroLength && _currentBaseAnimationId == SinbadAnimationID.ANIM_RUN_BASE)
			{
				// stop running if already moving and the player doesn't want to move
				SetBaseAnimation(SinbadAnimationID.ANIM_IDLE_BASE, false);
				if (_currentTopAnimationId == SinbadAnimationID.ANIM_RUN_TOP)
				{
					SetTopAnimation(SinbadAnimationID.ANIM_IDLE_TOP, false);
				}
			}
		}

		public void InjectMouseMove(object sender, MOIS.MouseEvent e)
		{
			// update camera goal based on mouse movement
			UpdateCameraGoal(-0.05f * e.state.X.rel, -0.05f * e.state.Y.rel, -0.0005f * e.state.Z.rel);
		}

		public void InjectMouseDown(MOIS.MouseButtonID id, MOIS.MouseEvent e)
		{
			if (_swordsDrawn && (_currentTopAnimationId == SinbadAnimationID.ANIM_IDLE_TOP || _currentTopAnimationId == SinbadAnimationID.ANIM_RUN_TOP))
			{
				// if swords are out, and character's not doing something weird, then SLICE!
				if (id == MOIS.MouseButtonID.MB_Left) 
				{
					SetTopAnimation(SinbadAnimationID.ANIM_SLICE_VERTICAL, true);
				}
				else if (id == MOIS.MouseButtonID.MB_Right) 
				{
					SetTopAnimation(SinbadAnimationID.ANIM_SLICE_HORIZONTAL, true);
				}

				_timer = 0;
			}
		}
		#endregion

        protected override void SetupBody()
		{
			//Create our main model
			_bodyNode = SceneManager.RootSceneNode.CreateChildSceneNode(Vector3.UNIT_Y * CharacterHeight);
			_bodyEntity = SceneManager.CreateEntity("SinbadBody", "Sinbad.mesh");
			_bodyNode.AttachObject(_bodyEntity);

			//Create swords and attach to sheath
			_sword1 = SceneManager.CreateEntity("SinbadSword1", "Sword.mesh");
			_sword2 = SceneManager.CreateEntity("SinbadSword2", "Sword.mesh");
			_bodyEntity.AttachObjectToBone("Sheath.L", _sword1);
			_bodyEntity.AttachObjectToBone("Sheath.R", _sword2);

			_verticalVelocity = 0f;
			_keyDirection = Vector3.ZERO;
		}

		protected void SetupCamera(Camera camera)
		{
			// create a pivot at roughly the character's shoulder
			_cameraPivot = camera.SceneManager.RootSceneNode.CreateChildSceneNode();
			// this is where the camera should be soon, and it spins around the pivot
			_cameraGoal = _cameraPivot.CreateChildSceneNode(new Vector3(0, 0, 15));
			// this is where the camera actually is
			_cameraNode = camera.SceneManager.RootSceneNode.CreateChildSceneNode();
			_cameraNode.Position = _cameraPivot.Position + _cameraGoal.Position;

			_cameraPivot.SetFixedYawAxis(true);
			_cameraGoal.SetFixedYawAxis(true);
			_cameraNode.SetFixedYawAxis(true);

			// our model is quite small, so reduce the clipping planes
			camera.NearClipDistance = 0.1f;
			camera.FarClipDistance = 100;
			_cameraNode.AttachObject(camera);

			_pivotPitch = 0f;
		}

		protected void SetupAnimations()
		{
			_bodyEntity.Skeleton.BlendMode = SkeletonAnimationBlendMode.ANIMBLEND_CUMULATIVE;

			string[] animNames =
				{"IdleBase", "IdleTop", "RunBase", "RunTop", "HandsClosed", "HandsRelaxed", "DrawSwords",
				"SliceVertical", "SliceHorizontal", "Dance", "JumpStart", "JumpLoop", "JumpEnd"};

			_animations = new AnimationState[NumberOfAnimations];
			_fadingIn = new bool[NumberOfAnimations];
			_fadingOut = new bool[NumberOfAnimations];

			// populate our animation list
			for (int i = 0; i < NumberOfAnimations; i++)
			{
				_animations[i] = _bodyEntity.GetAnimationState(animNames[i]);
				_animations[i].Loop = true;
				_fadingIn[i] = false;
				_fadingOut[i] = false;
			}
			
			//start idle animations
			SetBaseAnimation(SinbadAnimationID.ANIM_IDLE_BASE, false);
			SetTopAnimation(SinbadAnimationID.ANIM_IDLE_TOP, false);

			//relax hands
			_animations[(int)SinbadAnimationID.ANIM_HANDS_RELAXED].Enabled = true;

			_swordsDrawn = false;
		}

        private void UpdateBody(float deltaTime)
	    {
		    _goalDirection = Vector3.ZERO;

		    if (_keyDirection != Vector3.ZERO && _currentBaseAnimationId != SinbadAnimationID.ANIM_DANCE)
		    {
			    // calculate actually goal direction in world based on player's key directions
			    _goalDirection += _keyDirection.z * _cameraNode.Orientation.ZAxis;
			    _goalDirection += _keyDirection.x * _cameraNode.Orientation.XAxis;
			    _goalDirection.y = 0;
			    _goalDirection.Normalise();

			    Quaternion toGoal = _bodyNode.Orientation.ZAxis.GetRotationTo(_goalDirection);

			    // calculate how much the character has to turn to face goal direction
			    float yawToGoal = toGoal.Yaw.ValueDegrees;
			    // this is how much the character CAN turn this frame
			    float yawAtSpeed = yawToGoal / Mogre.Math.Abs(yawToGoal) * deltaTime * TurnSpeed;
			    // reduce "turnability" if we're in midair
			    if (_currentBaseAnimationId == SinbadAnimationID.ANIM_JUMP_LOOP) 
                {
                    yawAtSpeed *= 0.2f;
                }

			    // turn as much as we can, but not more than we need to
			    if (yawToGoal < 0) 
                {
                    yawToGoal = System.Math.Min(0, System.Math.Max(yawToGoal, yawAtSpeed)); //yawToGoal = Math::Clamp<Real>(yawToGoal, yawAtSpeed, 0);
                }
			    else if (yawToGoal > 0)
                {
                    yawToGoal = System.Math.Min(0, System.Math.Min(yawToGoal, yawAtSpeed)); //yawToGoal = Math::Clamp<Real>(yawToGoal, 0, yawAtSpeed);
                }
			
			    _bodyNode.Yaw(new Degree(yawToGoal));

			    // move in current body direction (not the goal direction)
			    _bodyNode.Translate(0, 0, deltaTime * RunSpeed * _animations[(int)_currentBaseAnimationId].Weight, Node.TransformSpace.TS_LOCAL);
		    }

		    if (_currentBaseAnimationId == SinbadAnimationID.ANIM_JUMP_LOOP)
		    {
			    // if we're jumping, add a vertical offset too, and apply gravity
			    _bodyNode.Translate(0, _verticalVelocity * deltaTime, 0, Node.TransformSpace.TS_LOCAL);
			    _verticalVelocity -= Gravity * deltaTime;
			
			    Vector3 pos = _bodyNode.Position;
			    if (pos.y <= CharacterHeight)
			    {
				    // if we've hit the ground, change to landing state
                    pos.y = CharacterHeight;
				    _bodyNode.Position = pos;
				    SetBaseAnimation(SinbadAnimationID.ANIM_JUMP_END, true);
				    _timer = 0;
			    }
		    }
	    }

        private void UpdateAnimations(float deltaTime)
	    {
		    float baseAnimSpeed = 1;
		    float topAnimSpeed = 1;

		    _timer += deltaTime;

		    if (_currentTopAnimationId == SinbadAnimationID.ANIM_DRAW_SWORDS)
		    {
			    // flip the draw swords animation if we need to put it back
			    topAnimSpeed = _swordsDrawn ? -1 : 1;

                float halfLength = _animations[(int)_currentTopAnimationId].Length / 2;
			    // half-way through the animation is when the hand grasps the handles...
			    if (_timer >=  halfLength && _timer - deltaTime < halfLength)
			    {
				    // so transfer the swords from the sheaths to the hands
				    _bodyEntity.DetachAllObjectsFromBone();
				    _bodyEntity.AttachObjectToBone(_swordsDrawn ? "Sheath.L" : "Handle.L", _sword1);
				    _bodyEntity.AttachObjectToBone(_swordsDrawn ? "Sheath.R" : "Handle.R", _sword2);
				    // change the hand state to grab or let go
				    _animations[(int)SinbadAnimationID.ANIM_HANDS_CLOSED].Enabled = !_swordsDrawn;
				    _animations[(int)SinbadAnimationID.ANIM_HANDS_RELAXED].Enabled = _swordsDrawn;
			    }

			    if (_timer >= _animations[(int)_currentTopAnimationId].Length)
			    {
				    // animation is finished, so return to what we were doing before
			    	if (_currentBaseAnimationId == SinbadAnimationID.ANIM_IDLE_BASE) 
                    {
                        SetTopAnimation(SinbadAnimationID.ANIM_IDLE_TOP, false);
                    }
				    else
				    {
					    SetTopAnimation(SinbadAnimationID.ANIM_RUN_TOP, false);
					    _animations[(int)SinbadAnimationID.ANIM_RUN_TOP].TimePosition = 
                            _animations[(int)SinbadAnimationID.ANIM_RUN_BASE].TimePosition;
				    }

				    _swordsDrawn = !_swordsDrawn;
			    }
		    }
		    else if (_currentTopAnimationId == SinbadAnimationID.ANIM_SLICE_VERTICAL || _currentTopAnimationId == SinbadAnimationID.ANIM_SLICE_HORIZONTAL)
		    {
			    if (_timer >= _animations[(int)_currentTopAnimationId].Length)
			    {
				    // animation is finished, so return to what we were doing before
				    if (_currentBaseAnimationId == SinbadAnimationID.ANIM_IDLE_BASE) 
                    {
                        SetTopAnimation(SinbadAnimationID.ANIM_IDLE_TOP, false);
                    }
				    else
				    {
					    SetTopAnimation(SinbadAnimationID.ANIM_RUN_TOP, false);
					    _animations[(int)SinbadAnimationID.ANIM_RUN_TOP].TimePosition = 
                            _animations[(int)SinbadAnimationID.ANIM_RUN_BASE].TimePosition;
				    }
			    }

			    // don't sway hips from side to side when slicing. that's just embarrasing.
			    if (_currentBaseAnimationId == SinbadAnimationID.ANIM_IDLE_BASE) 
                {
                    baseAnimSpeed = 0;
                }
		    }
		    else if (_currentBaseAnimationId == SinbadAnimationID.ANIM_JUMP_START)
		    {
			    if (_timer >= _animations[(int)_currentBaseAnimationId].Length)
			    {
				    // takeoff animation finished, so time to leave the ground!
				    SetBaseAnimation(SinbadAnimationID.ANIM_JUMP_LOOP, true);
				    // apply a jump acceleration to the character
				    _verticalVelocity = JumpAcceleration;
			    }
		    }
		    else if (_currentBaseAnimationId == SinbadAnimationID.ANIM_JUMP_END)
		    {
			    if (_timer >= _animations[(int)_currentBaseAnimationId].Length)
			    {
				    // safely landed, so go back to running or idling
				    if (_keyDirection == Vector3.ZERO)
				    {
					    SetBaseAnimation(SinbadAnimationID.ANIM_IDLE_BASE, false);
                        SetTopAnimation(SinbadAnimationID.ANIM_IDLE_TOP, false);
				    }
				    else
				    {
                        SetBaseAnimation(SinbadAnimationID.ANIM_RUN_BASE, true);
                        SetTopAnimation(SinbadAnimationID.ANIM_RUN_TOP, true);
				    }
			    }
		    }

		    // increment the current base and top animation times
            if (_currentBaseAnimationId != SinbadAnimationID.ANIM_NONE)
            {
                _animations[(int)_currentBaseAnimationId].AddTime(deltaTime * baseAnimSpeed);
            }

            if (_currentBaseAnimationId != SinbadAnimationID.ANIM_NONE)
            {
                _animations[(int)_currentBaseAnimationId].AddTime(deltaTime * topAnimSpeed);
            }
                
		    // apply smooth transitioning between our animations
		    FadeAnimations(deltaTime);
	    }

        private void FadeAnimations(float deltaTime)
	    {
		    for (int i = 0; i < NumberOfAnimations; i++)
		    {
			    if (_fadingIn[i])
			    {
				    // slowly fade this animation in until it has full weight
				    float newWeight = _animations[i].Weight + deltaTime * AnitmationFadeSpeed;
				    _animations[i].Weight = System.Math.Min(0, System.Math.Min(newWeight, 1));
				    if (newWeight >= 1) 
                    {
                        _fadingIn[i] = false;
                    }
			    }
			    else if (_fadingOut[i])
			    {
				    // slowly fade this animation out until it has no weight, and then disable it
                    float newWeight = _animations[i].Weight - deltaTime * AnitmationFadeSpeed;
                    _animations[i].Weight = System.Math.Min(0, System.Math.Min(newWeight, 1));
				    if (newWeight <= 0)
				    {
                        _animations[i].Enabled = false;
					    _fadingOut[i] = false;
				    }
			    }
		    }
	    }

        private void UpdateCamera(float deltaTime)
	    {
		    // place the camera pivot roughly at the character's shoulder
		    _cameraPivot.Position = _bodyNode.Position + Vector3.UNIT_Y * CameraHeight;
		    // move the camera smoothly to the goal
		    Vector3 goalOffset = _cameraGoal._getDerivedPosition() - _cameraNode.Position;
		    _cameraNode.Translate(goalOffset * deltaTime * 9.0f);
		    // always look at the pivot
		    _cameraNode.LookAt(_cameraPivot._getDerivedPosition(), Node.TransformSpace.TS_WORLD);
	    }

        private void UpdateCameraGoal(float deltaYaw, float deltaPitch, float deltaZoom)
	    {
		    _cameraPivot.Yaw(new Degree(deltaYaw), Node.TransformSpace.TS_WORLD);

		    // bound the pitch
		    if (!(_pivotPitch + deltaPitch > 25 && deltaPitch > 0) &&
			    !(_pivotPitch + deltaPitch < -60 && deltaPitch < 0))
		    {
			    _cameraPivot.Pitch(new Degree(deltaPitch), Node.TransformSpace.TS_LOCAL);
			    _pivotPitch += deltaPitch;
		    }
    		
		    float dist = (_cameraGoal._getDerivedPosition() - _cameraPivot._getDerivedPosition()).Length;
		    float distChange = deltaZoom * dist;

		    // bound the zoom
		    if (!(dist + distChange < 8 && distChange < 0) &&
			    !(dist + distChange > 25 && distChange > 0))
		    {
			    _cameraGoal.Translate(0, 0, distChange, Node.TransformSpace.TS_LOCAL);
		    }
	    }

		private void SetBaseAnimation(SinbadAnimationID animationId, bool reset)
		{
			if (_currentBaseAnimationId >= 0 && (int)_currentBaseAnimationId < NumberOfAnimations)
			{
				_fadingIn[(int)_currentBaseAnimationId] = false;
				_fadingOut[(int)_currentBaseAnimationId] = true;
			}

			_currentBaseAnimationId = animationId;

			if (animationId != SinbadAnimationID.ANIM_NONE)
			{
				// if we have a new animation, enable it and fade it in
				_animations[(int)animationId].Enabled = true;
				_animations[(int)animationId].Weight = 0;
				_fadingOut[(int)animationId] = false;
				_fadingIn[(int)animationId] = true;

				if (reset)
				{
					_animations[(int)animationId].TimePosition = 0;
				}
			}
		}

		private void SetTopAnimation(SinbadAnimationID animationId, bool reset)
		{
			if (_currentTopAnimationId >= 0 && (int)_currentTopAnimationId < NumberOfAnimations)
			{
				_fadingIn[(int)_currentTopAnimationId] = false;
				_fadingOut[(int)_currentTopAnimationId] = true;
			}

			_currentTopAnimationId = animationId;

			if (animationId != SinbadAnimationID.ANIM_NONE)
			{
				// if we have a new animation, enable it and fade it in
				_animations[(int)animationId].Enabled = true;
				_animations[(int)animationId].Weight = 0;
				_fadingOut[(int)animationId] = false;
				_fadingIn[(int)animationId] = true;

				if (reset)
				{
					_animations[(int)animationId].TimePosition = 0;
				}
			}
		}
	}
}
