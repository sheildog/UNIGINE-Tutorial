using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Unigine;
using UnigineApp.Tool;

namespace UnigineApp
{
    class AppWorldLogic : WorldLogic
    {
        // World logic, it takes effect only when the world is loaded.
        // These methods are called right after corresponding world script's (UnigineScript) methods.
        App app;
        Editor editor;
        List<ObjectMeshDynamic> Objects = new List<ObjectMeshDynamic>();
        Materials materials;
        Properties properties;

        public LightWorld lightWorld;
        public LightProj lightProj;
        public LightOmni lightOmni;
        float ifps;

        const float deltaAngle = 60f;
        const float movingSpeed = 3f;
        const float changeInterval = 1f;
        const float sunRotationRate = 10f;

        vec3 currentObjectsScale = new vec3(1f);
        dvec3 forwardDirection = new dvec3(0f, -1f, 0f);
        private float sunAngle = 0f;
        private float elapsedTime = changeInterval;

        Unigine.Object oldSelection;
        Unigine.Object newSelection;

        //GUI变量
        private Gui gui;
        private UserInterface ui;
        private WidgetLabel widget_label;
        private WidgetSlider widget_slider;
        private WidgetCheckBox widget_checkbox;
        private WidgetButton widget_button_del;
        private WidgetButton widget_button_close;
        private WidgetButton widget_button_fopen;
        private WidgetButton widget_button_wload;
        private WidgetDialogFile widget_file_dialog;
        private Widget mesh_parameters_window;

        //GUI事件
        static void onButtonDelClicked()
        {
            
        }

        //int onButtonCloseClicked();
        //int onButtonFopenClicked();
        //int onButtonWloadClicked();
        //int onDlgCancelClicked();
        //int onDlgOKClicked();
        //int onMeshParamsOKClicked();
        //int onSliderChanged();

        public int InitGUI()
        {
            gui = Gui.get();
            widget_label = new WidgetLabel(gui, "sun position");
            widget_label.setToolTip("Change the sun's position");
            widget_label.setPosition(10,10);

            widget_slider = new WidgetSlider(gui, 0, 360, 60);
            widget_slider.setToolTip("Slide to change sun position");
            widget_slider.setPosition(100,10);
            //widget_slider.setCallback0(Gui.CHANGED, new Widget.Callback0(this, onSliderChanged));

            widget_button_del = new WidgetButton(gui, "Delete");
            widget_button_del.setToolTip("Delect object(s) selected in combo box");
            widget_button_del.setPosition(170, 50);
            UserInterface.Callback0 c0 = new UserInterface.Callback0(onButtonDelClicked);
            widget_button_del.setCallback0(Gui.CLICKED,);

            widget_button_close = new WidgetButton(gui, "Delete");
            widget_button_close.setToolTip("Delect object(s) selected in combo box");
            widget_button_close.setPosition(170, 50);
            //widget_button_close.setCallback0(Gui.CLICKED, new Widget.Callback0(this, OnButtonDelClicked()));

            widget_button_fopen = new WidgetButton(gui, "Delete");
            widget_button_fopen.setToolTip("Delect object(s) selected in combo box");
            widget_button_fopen.setPosition(170, 50);
            //widget_button_fopen.setCallback0(Gui.CLICKED, new Widget.Callback0(this, OnButtonDelClicked()));

            widget_button_wload = new WidgetButton(gui, "Delete");
            widget_button_wload.setToolTip("Delect object(s) selected in combo box");
            widget_button_wload.setPosition(170, 50);
            //widget_button_wload.setCallback0(Gui.CLICKED, new Widget.Callback0(this, OnButtonDelClicked()));

            return 1;
        }

        public int SelectObject(Unigine.Object new_select_object, Unigine.Object old_select_object)
        {
            if (new_select_object != null)
            {
                if (new_select_object.getProperty(0).findParameter("selected") != -1)
                {
                    new_select_object.getProperty(0).setParameterInt(new_select_object.getProperty(0).findParameter("selected"), 1);
                    if (old_select_object != null)
                    {
                        //oldSelection1.getProperty().findParameter("selected");
                        old_select_object.getProperty(0).setParameterInt(old_select_object.getProperty(0).findParameter("selected"), 0);
                    }
                    oldSelection= new_select_object;
                    //Log.message("\n" + new_select_object.getName() + " is selected! \n");
                }
                return 1;
            }
            return 0;
        }

        public int AddMeshToScene(string fileName, string meshName, string materialName, dvec3 positon)
        {
            Mesh mesh = new Mesh();
            ObjectMeshDynamic objectMeshDynamic;
            string new_property_name = string.Format(meshName + "_property");

            if (fileName != null)
            {
                if (mesh.ToString() != mesh.load(fileName).ToString())
                {
                    Log.error("\n" + "Mesh文件打开错误" + "\n");
                    mesh.clear();
                    return 0;
                }
                else
                {
                    objectMeshDynamic = new ObjectMeshDynamic(mesh);
                }
            }
            else
            {
                mesh.addBoxSurface("boxSurface", new vec3(0.5f,0.5f,0.5f));
                objectMeshDynamic = new ObjectMeshDynamic(mesh);
            }
            objectMeshDynamic.setMaterial(materialName, "*");
            objectMeshDynamic.setName(meshName);
            objectMeshDynamic.setWorldPosition(positon);

            properties.inheritProperty("surface_base_1", "my_property_lib.prop", new_property_name);

            objectMeshDynamic.setProperty(new_property_name,"*");
            objectMeshDynamic.getProperty(0).setParameterString(objectMeshDynamic.getProperty(0).findParameter("material"), materialName);
            objectMeshDynamic.getProperty(0).setParameterInt(objectMeshDynamic.getProperty(0).findParameter("selected"), 0);

            objectMeshDynamic.setIntersection(1,0);

            editor.addNode(objectMeshDynamic.getNode());
            Objects.Add(objectMeshDynamic);
            Log.message("\n" + objectMeshDynamic + " add to the scene! \n");

            return 1;
        }

        public int InitObject()
        {
            int index = 0;

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    AddMeshToScene(null, string.Format("my_meshdynamic_"+ index), string.Format("my_mesh_base"+ index), new dvec3(i, j, 1f));
                    index++;
                }
            }
            Log.warning("\n" + "Objects generation is OK \n");

            return 1;
        }

        public int InitPlayer()
        {
            PlayerSpectator player = new PlayerSpectator();
            Camera camera = new Camera();

            player.setFov(90);
            player.setZNear(0.1f);
            player.setZFar(10000.0f);
            player.setPosition(new dvec3(3.0f));
            player.setDirection(new vec3(-1.0f), vec3.UP);
            Game.get().setPlayer(player.getPlayer());

            player.release();
            Log.warning("\n" + "Player is initialized successfully \n");

            return 1;
        }

        public int InitProjectedLight()
        {
            lightProj = new LightProj(new vec4(1.0f, 1.0f, 0.5f, 1.0f), 10, 60, "");
            lightProj.setWorldPosition(new dvec3(2.5f, 2.5f, 3.0f));
            lightProj.setName("projector");
            lightProj.setRotation(new quat(-45.0f, 45.0f, 0f));
            lightProj.setPenumbra(0.425f);
            lightProj.setIntensity(1.0f);
            lightProj.release();
            editor.addNode(lightProj.getNode());
            Log.message("\n" + "Created a projected light source \n");

            lightOmni = new LightOmni(new vec4(1f), 20f, null);
            lightOmni.setWorldPosition(new dvec3(0, 0, 5));
            lightOmni.setIntensity(0.1f);
            lightOmni.release();
            editor.addNode(lightOmni.getNode());
            Log.message("\n" + "Created a omni light source \n");

            lightWorld = new LightWorld(new vec4(1));
            lightWorld.setName("Sun");
            lightWorld.setDisableAngle(90f);
            lightWorld.setIntensity(1);
            lightWorld.setScattering(LightWorld.SCATTERING_SUN);
            lightWorld.setWorldRotation(new quat(0, 1, 0, 0));
            lightWorld.release();
            editor.addNode(lightWorld.getNode());
            Log.message("\nCreated a world light source\n");

            return 1;
        }

        public int InitMaterials()
        {
            materials = Materials.get();
            materials.create("my_material_lib");
            materials.inheritMaterial("mesh_base", "my_material_lib", "my_mesh_base0");
            Material my_mesh_base = materials.findMaterial("my_mesh_base0");
            my_mesh_base.setParameter(Material.PARAMETER_COLOR, new vec4(255, 0, 0, 255));
            Log.message("\nGenerated material "+ my_mesh_base.getName());

            materials.inheritMaterial("mesh_base", "my_material_lib", "my_mesh_base1");
            my_mesh_base = materials.findMaterial("my_mesh_base1");
            my_mesh_base.setParameter(Material.PARAMETER_COLOR, new vec4(0, 255, 0, 255));
            Log.message("\nGenerated material " + my_mesh_base.getName());

            materials.inheritMaterial("mesh_base", "my_material_lib", "my_mesh_base2");
            my_mesh_base = materials.findMaterial("my_mesh_base2");
            my_mesh_base.setParameter(Material.PARAMETER_COLOR, new vec4(0, 0, 255, 255));
            Log.message("\nGenerated material " + my_mesh_base.getName());

            materials.inheritMaterial("mesh_base", "my_material_lib", "my_mesh_base3");
            my_mesh_base = materials.findMaterial("my_mesh_base3");
            my_mesh_base.setParameter(Material.PARAMETER_COLOR, new vec4(255, 255, 0, 255));
            Log.message("\nGenerated material " + my_mesh_base.getName());

            Log.warning("\nMaterial generation is OK \n\n");

            my_mesh_base.clearPtr();
            return 1;
        }

        public int UpdateObjects()
        {
            ObjectMeshDynamic omd;
            for (int i = 0;i<Objects.Count;i++)
            {
                omd = Objects[i];
                Property p = omd.getProperty(0);
                if (p.checkPtr())
                {
                    int param = p.findParameter("selected");
                    if (param!=-1)
                    {
                        if (p.getParameterInt(param) ==1)
                        {
                            omd.setMaterial("mesh_base", "*");
                        }
                        else
                        {
                            omd.setMaterial(p.getParameterString(p.findParameter("material")), "*");
                            //omd.setMaterialParameter("albedo_color",new vec4(1,1,0,1),0 );
                        }
                    }
                }
            }

            //for (int i = 0; i < Objects.Count; i++)
            //{
            //    if (string.Format(Objects[i].getName()) == "my_meshdynamic_" + i)
            //    {
            //        //TransformNode(Objects[i].getNode(), ifps);
            //    }
            //}

            return 1;
        }

        public int ClearMaterials()
        {
            for (int i = 0; i <= 3; i++)
            {
                materials.removeMaterial("my_mesh_base" + i);
                Log.warning("\nmy_mesh_base" + i + " has been removed!\n");
            }

            return 1;
        }

        public int RemoveMeshFromScene(string nodeName)
        {
            ObjectMeshDynamic omd = ObjectMeshDynamic.cast(editor.getNodeByName(nodeName));
            if (omd !=null)
            {
                if (oldSelection !=null)
                {
                    if (omd.getNode() == oldSelection.getNode())
                    {
                        oldSelection.clearPtr();
                    }
                }
                Log.message("\nRemoving " + omd.getTypeName() + " node named " + nodeName + " from the scene.\n");
                for (int i = 0; i < Objects.Count; i++)
                {
                    if (string.Compare(Objects[i].getName(), nodeName) == 0)
                    {
                        Objects.Remove(Objects[i]);
                        break;
                    }
                }
                editor.removeNode(omd.getNode());
            }
            return 1;
        }

        public int RemoveObjects()
        {
            while (Objects.Count >0)
            {
                RemoveMeshFromScene(Objects[0].getName());
            }

            return 1;
        }

        public int TransformNode(Node node, float ifps)
        {

            float deltaAngle = 60;
            float moveSpeed = 3;
            dmat4 transform = node.getTransform();
            Random rand = new Random();
            
            quat deltaRotation = new quat(rand.Next(-2, 2), rand.Next(-2, 2), rand.Next(-2, 2), deltaAngle * ifps);



            node.setWorldScale(currentObjectsScale);
            node.setWorldRotation(node.getWorldRotation()*deltaRotation);
            node.setWorldPosition(node.getWorldPosition()+forwardDirection*ifps*moveSpeed);

            return 1;
        }

        public int UpdateLights()
        {
            sunAngle += sunRotationRate*ifps;
            if (sunAngle > 360)
            {
                sunAngle = 0;
            }
            LightWorld.cast(editor.getNodeByName("Sun")).setWorldRotation(new quat(new vec3(0, 1, 0), 180 - sunAngle));

            return 1;
        }



        public Unigine.Object GetObjectUnderCursor(Player player, int mouseX, int mouseY, float max_distance)
        {
            dvec3 p0 = player.getWorldPosition();
            dvec3 p1 = p0 + new dvec3(player.getDirectionFromScreen(mouseX, mouseY) * max_distance);
            WorldIntersection intersection = new WorldIntersection();

            return World.get().getIntersection(p0, p1, 1, intersection);
        }

        public int InitProperties()
        {
            properties = Properties.get();
            properties.addWorldLibrary("my_property_lib.prop");

            return 1;
        }

        public int ClearProperties()
        {
            properties.clear("my_property_lib.prop");
            properties.removeWorldLibrary("my_property_lib.prop");

            return 1;
        }

        //GUI Event Handler


        public AppWorldLogic()
        {
        }

        public override int init()
        {
            app = App.get();
            editor = Editor.get();
            Game.get().setSeed(2);

            InitGUI();
            InitProperties();
            InitMaterials();
            InitObject();
            InitPlayer();
            InitProjectedLight();
            
            return 1;
        }

        // start of the main loop
        public override int update()
        {
            ifps = Game.get().getIFps();

            if (elapsedTime <0.0f)
            {
                currentObjectsScale = new vec3(Game.get().getRandomFloat(0.8f,1.2f));
                forwardDirection = forwardDirection*MathLib.rotateZ(60);
                elapsedTime = changeInterval;
            }
            elapsedTime -= ifps;

            if (app.clearMouseButtonState(App.BUTTON_RIGHT) == 1)
            {
                newSelection = GetObjectUnderCursor(Game.get().getPlayer(), app.getMouseX(), app.getMouseY(), 100f);
                SelectObject(newSelection, oldSelection);
            }
            if (app.getKeyState('q') ==1 && Unigine.Console.get().getActivity() !=1)
            {
                app.exit();
            }

            UpdateObjects();
            UpdateLights();

            return 1;
        }

        public override int render()
        {
            // The engine calls this function before rendering each render frame: correct behavior after the state of the node has been updated.

            return 1;
        }

        public override int flush()
        {
            // Write here code to be called before updating each physics frame: control physics in your application and put non-rendering calculations.
            // The engine calls flush() with the fixed rate (60 times per second by default) regardless of the FPS value.
            // WARNING: do not create, delete or change transformations of nodes here, because rendering is already in progress.

            return 1;
        }


        public override int shutdown()
        {
            // Write here code to be called on world shutdown: delete resources that were created during world script execution to avoid memory leaks.
            for (int i = 0; i < Objects.Count; i++)
            {
                editor.removeNode(Objects[i].getNode());
            }
            Objects.Clear();

            lightWorld.clearPtr();
            lightOmni.clearPtr();
            lightProj.clearPtr();

            ClearMaterials();
            ClearProperties();

            RemoveObjects();

            return 1;
        }

        public override int destroy()
        {
            // Write here code to be called when the video mode is changed or the application is restarted (i.e. video_restart is called). It is used to reinitialize the graphics context.

            return 1;
        }

        public override int save(Stream stream)
        {
            // Write here code to be called when the world is saving its state (i.e. state_save is called): save custom user data to a file.

            return 1;
        }

        public override int restore(Stream stream)
        {
            // Write here code to be called when the world is restoring its state (i.e. state_restore is called): restore custom user data to a file here.

            return 1;
        }
    }
}
