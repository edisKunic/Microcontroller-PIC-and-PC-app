/* File:   main.c
 * Author: Edis Kunic
 * Created on January 11, 2015, 11:22 PM*/

#include <xc.h>
#pragma config FOSC=HS,WDTE=OFF,PWRTE=OFF,MCLRE=ON,CP=OFF,CPD=OFF,BOREN=OFF,CLKOUTEN=OFF
#pragma config IESO=OFF,FCMEN=OFF,WRT=OFF,VCAPEN=OFF,PLLEN=OFF,STVREN=OFF,LVP=OFF
#define _XTAL_FREQ 8000000
#include "CLI.h"
/*------------------TASKOVI------------------*/

void dioda_1(){
    PORTBbits.RB0= !(PORTBbits.RB0);
}
void dioda_2(){
    PORTBbits.RB2= !(PORTBbits.RB2);
}
void dioda_3(){
    PORTBbits.RB5= !(PORTBbits.RB5);
}
void dioda_4(){
    PORTBbits.RB1= !(PORTBbits.RB1);
}

/*--------------------------------------------*/
void inicializiraj_portove(){

    TRISD=0x00;
    ANSELD=0x00;
    LATD=0x00;
    PORTD=0x00;

    TRISB=0x00;
    ANSELB=0x00;
    LATB=0x00;
    PORTB=0x00;

    PORTBbits.RB0=0;
    PORTBbits.RB2=0;
    PORTBbits.RB5=0;

}
void main(void) {
    konfiguracija();
    inicializiraj_portove();
    Ticker_attach_ms(&dioda_1,100);      // RB0
    Ticker_attach(&dioda_2,1);      // RB2
    Ticker_attach(&dioda_3,2);
    Ticker_attach_ms(&dioda_4,10);
    command_interface();

    return;
}