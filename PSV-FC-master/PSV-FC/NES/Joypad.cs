// created on 11/27/2004 at 9:32 AM
using System;
using System.IO;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;
//using Tao.Sdl;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Input;

public class Joypad
{
    enum Button
    {
        BUTTON_A = 1,
        BUTTON_B = 2,
        BUTTON_SELECT = 4,
        BUTTON_START = 8,
        BUTTON_UP = 16,
        BUTTON_DOWN = 32,
        BUTTON_LEFT = 64,
        BUTTON_RIGHT = 128
    };

    byte joypad_lastwrite;
    int joypad1_readpointer;
    int joypad2_readpointer;

    byte joypad1_state;
    byte joypad2_state;

    private byte InternalGetJoyState()
    {
        //int numberOfKeys;
        ////FIXME: This is SDL-centric for the time being
        //byte[] keystate = Sdl.SDL_GetKeyState(out numberOfKeys);
        byte result = 0;
		var gamePadData = GamePad.GetData(0);
		
		if((gamePadData.Buttons & GamePadButtons.Up) != 0)
			result |= (byte)Button.BUTTON_UP;
		else if ((gamePadData.Buttons & GamePadButtons.Down) != 0)
            result |= (byte)Button.BUTTON_DOWN;
		if((gamePadData.Buttons & GamePadButtons.Left) != 0)
			result |= (byte)Button.BUTTON_LEFT;
		else if ((gamePadData.Buttons & GamePadButtons.Right) != 0)
            result |= (byte)Button.BUTTON_RIGHT;
		
		if ((gamePadData.Buttons & GamePadButtons.Cross) != 0)
            result |= (byte)Button.BUTTON_A;
        if ((gamePadData.Buttons & GamePadButtons.Square) != 0)
            result |= (byte)Button.BUTTON_B;
        if ((gamePadData.Buttons & GamePadButtons.Select) != 0)
            result |= (byte)Button.BUTTON_SELECT;
        if ((gamePadData.Buttons & GamePadButtons.Start) != 0)
            result |= (byte)Button.BUTTON_START;
		
		
        return result;
    }
     
    public byte Joypad_1_Read()
    {
       byte returnedValue = 0;

        switch (joypad1_readpointer)
        {
            case (1): if ((joypad1_state & (byte)Button.BUTTON_A) == (byte)Button.BUTTON_A) { returnedValue = 1; }; break;
            case (2): if ((joypad1_state & (byte)Button.BUTTON_B) == (byte)Button.BUTTON_B) { returnedValue = 1; }; break;
            case (3): if ((joypad1_state & (byte)Button.BUTTON_SELECT) == (byte)Button.BUTTON_SELECT) { returnedValue = 1; }; break;
            case (4): if ((joypad1_state & (byte)Button.BUTTON_START) == (byte)Button.BUTTON_START) { returnedValue = 1; }; break;
            case (5): if ((joypad1_state & (byte)Button.BUTTON_UP) == (byte)Button.BUTTON_UP) { returnedValue = 1; }; break;
            case (6): if ((joypad1_state & (byte)Button.BUTTON_DOWN) == (byte)Button.BUTTON_DOWN) { returnedValue = 1; }; break;
            case (7): if ((joypad1_state & (byte)Button.BUTTON_LEFT) == (byte)Button.BUTTON_LEFT) { returnedValue = 1; }; break;
            case (8): if ((joypad1_state & (byte)Button.BUTTON_RIGHT) == (byte)Button.BUTTON_RIGHT) { returnedValue = 1; }; break;
        }
        joypad1_readpointer++;
        return returnedValue;
    }
	
    public void Joypad_Write(byte data)
    {
        if ((data == 0) && (joypad_lastwrite == 1))
        {
            joypad1_state = InternalGetJoyState();
            joypad1_readpointer = 1;
            //joypad2_state = InternalGetJoyState(PlayerIndex.Two);
            //joypad2_readpointer = 1;
        }
        joypad_lastwrite = data;
    }
   /*
    public byte Joypad_2_Read()
    {
        byte returnedValue = 0;

        switch (joypad2_readpointer)
        {
            case (1): if ((joypad2_state & (byte)Button.BUTTON_A) == (byte)Button.BUTTON_A) { returnedValue = 1; }; break;
            case (2): if ((joypad2_state & (byte)Button.BUTTON_B) == (byte)Button.BUTTON_B) { returnedValue = 1; }; break;
            case (3): if ((joypad2_state & (byte)Button.BUTTON_SELECT) == (byte)Button.BUTTON_SELECT) { returnedValue = 1; }; break;
            case (4): if ((joypad2_state & (byte)Button.BUTTON_START) == (byte)Button.BUTTON_START) { returnedValue = 1; }; break;
            case (5): if ((joypad2_state & (byte)Button.BUTTON_UP) == (byte)Button.BUTTON_UP) { returnedValue = 1; }; break;
            case (6): if ((joypad2_state & (byte)Button.BUTTON_DOWN) == (byte)Button.BUTTON_DOWN) { returnedValue = 1; }; break;
            case (7): if ((joypad2_state & (byte)Button.BUTTON_LEFT) == (byte)Button.BUTTON_LEFT) { returnedValue = 1; }; break;
            case (8): if ((joypad2_state & (byte)Button.BUTTON_RIGHT) == (byte)Button.BUTTON_RIGHT) { returnedValue = 1; }; break;
        }
        joypad2_readpointer++;
        return returnedValue;
    }*/
}