//Because RGB565 on PSV is BGR and on PC SIMULATOR is RGB , so USE different shader.
//FOR PC Simulator ,Comment out the following line.
#define BUILD_FOR_PSV

using System;
using System.IO;
using System.Collections.Generic;


using System.Diagnostics;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;
using Sce.PlayStation.Core.Imaging;

using Sample;

namespace PSVFC
{
	public class AppMain
	{
		private static GraphicsContext graphics;
		static Texture2D		texture;
		static VertexBuffer 	vertices;
		static ShaderProgram	program;
		private static NesEngine myEngine = new NesEngine();
        private static bool running = false;
		private static int  index = 0;
		private static Stopwatch  stopWatch;
		private static double       mFps;
		
		private static long        mFrame = 0;
		private static long        mT = 0;
		private static long        mT0 = 0;
		
		static string[] romFiles;
		static  int    numOfRom = 0;
		static  int    curIndex = 0;
		static  bool   btnFlags = false;
		public static void Main (string[] args)
		{
			Initialize ();
			GetNesFiles();
			LoadCart(@"/Application/Rom/"+romFiles[curIndex]);
			stopWatch=new Stopwatch();
			stopWatch.Start();

			while (true) {
				SystemEvents.CheckEvents ();
				Update ();
				Render ();
				mFrame++;
			    mT = stopWatch.ElapsedMilliseconds ;
			    if (mT - mT0 >= 1000) {
				 float seconds = (mT - mT0)/1000;
				 mFps = mFrame / seconds; 
				 mT0    = mT;
				 mFrame = 0;
			  }   
			}
		}

		public static void Initialize ()
		{
			// Set up the graphics system
			graphics = new GraphicsContext ();
#if BUILD_FOR_PSV
			program = new ShaderProgram("/Application/shaders/Texture.cgx");
#else
			program = new ShaderProgram("/Application/shaders/Texture_sim.cgx");
#endif
			program.SetUniformBinding(0, "WorldViewProj");
			program.SetAttributeBinding(0, "a_Position");
			program.SetAttributeBinding(1, "a_TexCoord");
			vertices = new VertexBuffer(4, VertexFormat.Float3, VertexFormat.Float2);

			float[] positions = {
				-1.0f, 1.0f, 0.0f,
				-1.0f, -1.0f, 0.0f,
				1.0f, 1.0f, 0.0f,
				1.0f, -1.0f, 0.0f,
			};
			float[] texcoords = {
				0.0f, 0.0f,
				0.0f, 1.0f,
				1.0f, 0.0f,
				1.0f, 1.0f,
			};
			vertices.SetVertices(0, positions);
			vertices.SetVertices(1, texcoords);
			texture = new Texture2D(256, 224, false, PixelFormat.Rgb565);
			
			SampleDraw.Init(graphics);
			
		}
		
		private static void GetNesFiles()
		{
			DirectoryInfo dirInfo = new DirectoryInfo(@"/Application/Rom");
			FileInfo[] fileInfos = dirInfo.GetFiles(@"*.nes");
			//get number of the .nes file
			foreach (FileInfo fInfo in fileInfos)
            {
						numOfRom++;
            }
			//put names in romFiles
			romFiles = new string[numOfRom];
			foreach (FileInfo fInfo in fileInfos)
            {
						romFiles[curIndex] = fInfo.Name;
						curIndex++;
            }
			//we begin the first game
			curIndex = 0;
		}
		
		private static void LoadCart(string filename)
        {
            myEngine.LoadCart(filename);
            myEngine.LoadRam();
            myEngine.StartCart();
            running = true;
        }
		
		public static void Update ()
		{
			// Query gamepad for current state
			if (running)
            {
                UpdateGame();
            }
			
		}

		public static void Render ()
		{
			graphics.Clear ();
			DrawGame();
			// Present the screen
			graphics.SwapBuffers ();
		}
		
		private static void UpdateGame()
        {
            var gamePadData = GamePad.GetData(0);
			if ((gamePadData.Buttons & GamePadButtons.Triangle) != 0){
				btnFlags = true;	
			}
			
			if(btnFlags == true && (gamePadData.Buttons & GamePadButtons.Triangle) == 0){
				curIndex++;
				if(curIndex >=numOfRom ){
					curIndex = 0;	
				}
				Console.WriteLine(curIndex);
				myEngine.StopCart();
				LoadCart(@"/Application/Rom/"+romFiles[curIndex]);
				btnFlags = false;
			}
				
            myEngine.RunCart();
        }

        private static void DrawGame()
        {
			Matrix4 WorldMatrix;
			WorldMatrix = Matrix4.Identity;
			program.SetUniformValue(0, ref WorldMatrix);
			graphics.SetShaderProgram(program);
			graphics.SetVertexBuffer(0, vertices);
			graphics.SetTexture(0, texture);
		    texture.SetPixels(0,myEngine.myPPU.offscreenBuffer,0,0,256,224);
			graphics.DrawArrays(DrawMode.TriangleStrip, 0, 4);		
			SampleDraw.DrawText(mFps.ToString(),0xffffffff,0,0);
        }
		
		
	}
}
