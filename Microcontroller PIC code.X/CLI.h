/* 
 * File:   CLI.h
 * Author: Edis Kunic
 *
 * Created on August 5, 2015, 4:30 AM
 */

#ifndef CLI_H
#define	CLI_H

int br_funkcija=0;                  // brojac dodjeljenih funkcija
struct prekid{                      // ---- struktura za obradu prekida ----
    void(*f)(void);                 // pokazivac na funckiju
    unsigned int vrijeme_ms;        // period pozivanja funkcije
    int zauzeto;                    // provjera da li je zazeto ili ne
    int start;
    unsigned int brojac;            // brojac prekida
};
char cifre[10]={0, 1, 2, 3, 4, 5, 6, 7, 8, 9};
struct prekid niz[5];    // niz od 5 struktura koje omogucavaju periodicno izvrsavanje do 5 funkcija
void up_date_time(int br_taska, int t, int ms_or_sec);
int read_number();
void write_number(int broj);
void aktiviraj(int br_task){
    niz[br_task].start=1;
}
void deaktiviraj(int br_task){
    niz[br_task].start=0;
}
void write_char(char c){
    while(!TXIF);
    TXREG=c;

}
unsigned char read_char(){
    while(!RCIF);
    return RCREG;
}
void command_interface(){
    char naredba;
    int br_taska;
    char br;
    int t;
    int ms_or_sec;
    while(1){
        naredba=read_char();
        switch (naredba){
            case 'A':
                br=read_char();
                br_taska=br -'0';
                aktiviraj(br_taska);
                break;
            case 'D':
                br=read_char();
                br_taska=br -'0';
                deaktiviraj(br_taska);
                break;
            case 'C':
                write_char(br_funkcija);
                for(int i=0;i<5;i++){
                    write_char(niz[i].start);
                }
                for(int i=0;i<5;i++){
                    write_number(niz[i].vrijeme_ms);
                }
                break;
            case 'U':
                br=read_char();
                br_taska=br -'0';
                
                t=read_number();
                br=read_char();
                ms_or_sec=br -'0';
                up_date_time(br_taska,t,ms_or_sec);
                break;
        }
    }
}
void update_time(int br_taska, int t, int ms_or_sec){
    if(ms_or_sec==1){
        niz[br_taska].vrijeme_ms=t*1000;
        niz[br_taska].brojac=0;
    }else if(ms_or_sec==0){
        niz[br_taska].vrijeme_ms=t;
        niz[br_taska].brojac=0;
    }
    write_char('0');
}
int read_number(){
    char buffer[4];
    for(int i=0;i<4;i++){
        while(!RCIF);
        buffer[i]=RCREG;
    }
    int broj=0,t=1000;
    for(int i=0;i<4;i++){
        broj+=(buffer[i]-'0')*t;
        t=t/10;
    }
    return broj;

}
void write_number(int broj){
    int pom, t=10000;
    for(int i=0;i<5;i++){
        pom=(int)(broj/t);
        write_char(cifre[pom]);
        broj=broj-pom*t;
        t=t/10;
    }
    write_char('2');

}
void inicializiraj_tajmer(){        // funkcija za inicijalizaciuju tajemra na 1 ms
    OPTION_REGbits.PSA=0;           // ------------------
    OPTION_REGbits.TMR0CS=0;
    OPTION_REGbits.PS=0x02;         // preskaler postavljam na 8
    TMR0=0x06;                      // KONFIGURISANJE TAJMERA
    TMR0IF=0;                       //
    TMR0IE=1;
    GIE=1;                          // -------------------

    int i=0;
    for(i=0;i<5;i++){               // svi clanovi niza su slobodni na pocetku programa
        niz[i].zauzeto=0;
        niz[i].brojac=0;
        niz[i].vrijeme_ms=0;
        niz[i].start=0;		        //svi taskovi su u mirujucem modu
    }
}

void Ticker_attach(void(*f)(void), int vrijeme){ // funkcija za dodjeljivanje adrese i perioda u sekundama,
    int i;                                       // za raspolozivu strukturu
    for(i=0;i<5;i++){
        if(niz[i].zauzeto==0){
            niz[i].f=(*&f);
            niz[i].vrijeme_ms=vrijeme*1000;      // pretavaranje u ms
            niz[i].zauzeto=1;
            br_funkcija++;
            return;
        }
    }
}
void Ticker_attach_ms(void(*f)(void), int vrijeme){ // funkcija za dodjeljivanje adrese funkcije, sa peridodom u ms
    int i=0;
    for(i=0;i<5;i++){
        if(niz[i].zauzeto==0){
            niz[i].f=(*&f);
            niz[i].vrijeme_ms=vrijeme;
            niz[i].zauzeto=1;
            br_funkcija++;
            return;
        }
    }
}
void Ticker_deatach(void(*funkcija)(void)){ // funkcija za brisanje vec dodjelejnje funkcije
    for(int i=0;i<br_funkcija;i++){
        if(niz[i].f==funkcija){
            niz[i].zauzeto=0;
        }
    }
    br_funkcija--;
}
void interrupt tc_int(void) {
    if(TMR0IE && TMR0IF)
    {
        TMR0IF=0;
        for(int i=0;i<5;i++){
            if(niz[i].zauzeto==1 && niz[i].start==1) niz[i].brojac++;
        }
        if(niz[0].vrijeme_ms==niz[0].brojac && niz[0].zauzeto==1 && niz[0].start==1) { niz[0].brojac=0; niz[0].f(); }
        if(niz[1].vrijeme_ms==niz[1].brojac && niz[1].zauzeto==1 && niz[1].start==1) { niz[1].brojac=0; niz[1].f(); }
        if(niz[2].vrijeme_ms==niz[2].brojac && niz[2].zauzeto==1 && niz[2].start==1) { niz[2].brojac=0; niz[2].f(); }
        if(niz[3].vrijeme_ms==niz[3].brojac && niz[3].zauzeto==1 && niz[3].start==1) { niz[3].brojac=0; niz[3].f(); }
        if(niz[4].vrijeme_ms==niz[4].brojac && niz[4].zauzeto==1 && niz[4].start==1) { niz[4].brojac=0; niz[4].f(); }
        TMR0=0x06;
    }
    return;
}
/*----Konfiguracija serijskog porta i ostalih funkcija
 za komunikaciju sa racunarom------------------------*/
void init_serial()
{
    BRG16=0;
    BRGH=0;
    SPBRGL=12;
    SYNC=0;
    SPEN=1;

    TXEN=1;
    CREN=1;
    RCIE=0;
}
void konfiguracija(){
    
    inicializiraj_tajmer();
    init_serial();
}
#endif	/* CLI_H */

