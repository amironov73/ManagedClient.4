//============================================================
// УПРОЩЕННАЯ ГРАММАТИКА ЯЗЫКА ФОРМАТИРОВАНИЯ ИРБИС
// грамматика для ANTLR 4.2
// Автор: А. В. Миронов
// Версия: 0.0.18
//============================================================

grammar Pft;

// Стартовый символ
program
        : statement* EOF
        ;

statement
        : nonGrouped
        | groupStatement
        ;

groupStatement
        : LPAREN nonGrouped RPAREN
        ;

nonGrouped
        : formatItem+   # FormatItemPlus
        ;


//============================================================
// ОБЫЧНОЕ ФОРМАТИРОВАНИЕ
//============================================================

formatItem
        : conditionalStatement       # ConditionalStatementOuter
        | fieldReference             # FieldReferenceOuter
        | formatExit                 # FormatExitOuter
        | error                      # ErrorOuter
        | warning                    # WarningOuter
        | fatal                      # FatalOuter
        | trace                      # TraceOuter
        | debug                      # DebugOuter
        | sFunction                  # SFunctionOuter
        | fFunction                  # FFunctionOuter
        | refFunction                # RefFunctionOuter
        | trimFunction               # TrimFunctionOuter
        | iocc                       # IoccOuter
        | nocc                       # NoccOuter
        | BREAK                      # Break
        | GLOBALVAR                  # GlobalReference
        | UNCONDITIONAL              # UnconditionalLiteral
        | MFN                        # SimpleMfn
        | MFNWITHLENGTH              # MfnWithLength
        | COMMA                      # Comma
        | SLASH                      # SlashNewLine
        | HASH                       # HashNewLine
        | PERCENT                    # PercentNewLine
        | MODESWITCH                 # ModeSwitch
        | COMMANDC                   # CommandC
        | COMMANDX                   # CommandX
        ;

fieldReference
        : leftHand
          FIELD
          rightHand
        ;

leftHand
        : (REPEATABLE PLUS?|CONDITIONAL)*
        ;

rightHand
        : (PLUS? REPEATABLE|CONDITIONAL)*
        ;

formatExit
        : FORMATEXIT LPAREN statement RPAREN
        ;

//============================================================
// УСЛОВНЫЙ ОПЕРАТОР
//============================================================

conditionalStatement
        : IF condition THEN thenBranch=statement* ( ELSE elseBranch=statement* )? FI
        ;

condition
        : condition op=(AND|OR) condition # ConditionAndOr
        | NOT condition                   # ConditionNot
        | LPAREN condition RPAREN         # ConditionParen
        | stringTest                      # ConditionString
        | fieldPresense                   # ConditionField
        | arithCondition                  # ConditionArith
        ;

stringTest
        : left=formatItem
          op=(COLON|EQUALS|NOTEQUALS|MORE|MOREEQ|LESS|LESSEQ)
          right=formatItem
        ;

arithCondition
        : left=arithExpr
          op=(EQUALS|NOTEQUALS|MORE|MOREEQ|LESS|LESSEQ)
          right=arithExpr
        ;

arithExpr
        : left=arithExpr op=(STAR|SLASH) right=arithExpr
        | left=arithExpr op=(PLUS|MINUS) right=arithExpr
        | value
        ;

value
        : FLOAT                         # FloatValue
        | UNSIGNED                      # UnsignedValue
        | SIGNED                        # SignedValue
        | MINUS arithExpr               # MinusExpression
        | LPAREN arithExpr RPAREN       # ParenthesisExpression
        | arithFunction                 # ArithFunctionOuter
        | MFN                           # MfnValue
        ;

arithFunction
        : RSUM LPAREN nonGrouped RPAREN  # RsumFunction
        | RMAX LPAREN nonGrouped RPAREN  # RmaxFunction
        | RMIN LPAREN nonGrouped RPAREN  # RminFunction
        | RAVR LPAREN nonGrouped RPAREN  # RavrFunction
        | VAL  LPAREN nonGrouped RPAREN  # ValFunction
        | L    LPAREN nonGrouped RPAREN  # LFunction
        ;

fieldPresense
        : P LPAREN fieldReference RPAREN
        | A LPAREN fieldReference RPAREN
        ;

//============================================================
// ФУНКЦИИ
//============================================================

sFunction
        : S LPAREN nonGrouped RPAREN
        ;

fFunction
        : F LPAREN arg1=arithExpr RPAREN
        | F LPAREN arg1=arithExpr COMMA arg2=arithExpr RPAREN
        | F LPAREN arg1=arithExpr COMMA arg2=arithExpr COMMA arg3=arithExpr RPAREN
        ;

refFunction
        : REF LPAREN arg1=arithExpr COMMA arg2=nonGrouped RPAREN
        ;

trimFunction
        : TRIM LPAREN nonGrouped RPAREN
        ;

iocc
        : IOCC
        ;

nocc
        : NOCC
        ;

//============================================================
// ОШИБКИ
//============================================================

error
        : ERROR LPAREN statement RPAREN
        ;

warning
        : WARNING LPAREN statement RPAREN
        ;

fatal
        : FATAL LPAREN statement RPAREN
        ;

trace
        : TRACE LPAREN statement RPAREN
        ;

debug
        : DEBUG LPAREN statement RPAREN
        ;

//============================================================
// ТЕРМИНАЛЫ
//============================================================

UNCONDITIONAL
        : '\'' .*? '\''
        ;

CONDITIONAL
        : '"' .*? '"'
        ;

REPEATABLE
        : '|' .*? '|'
        ;

FIELD
        : [dvn] DIGIT+
          ( '@' DIGIT+ )?
          ( '^' ALNUM )?
          ( '[' (INTEGER | LAST) (MINUS INTEGER)? ']')?
          ( '*' INTEGER )?
          ( '.' INTEGER )?
        ;

GLOBALVAR
        : 'g' [0-9]+
          ( '^' ALNUM )?
          ( '*' INTEGER )?
          ( '.' INTEGER )?
        ;

MODESWITCH
        : [Mm][PpHhDd][UuLl]
        ;

MFNWITHLENGTH
        : [Mm][Ff][Nn][(][0-9]+[)]
        ;

COMMANDC: [Cc] INTEGER;
COMMANDX: [Xx] INTEGER;

LAST        : 'LAST';
MFN         : [Mm][Ff][Nn];
IF          : [Ii][Ff];
THEN        : [Tt][Hh][Ee][Nn];
ELSE        : [Ee][Ll][Ss][Ee];
FI          : [Ff][Ii];
AND         : [Aa][Nn][Dd];
OR          : [Oo][Rr];
NOT         : [Nn][Oo][Tt];
VAL         : [Vv][Aa][Ll];
REF         : [Rr][Ee][Ff];
RSUM        : [Rr][Ss][Uu][Mm];
RMAX        : [Rr][Mm][Aa][Xx];
RMIN        : [Rr][Mm][Ii][Nn];
RAVR        : [Rr][Aa][Vv][Rr];
S           : [Ss];
L           : [Ll];
F           : [Ff];
A           : [Aa];
P           : [Pp];
TRIM        : [Tt][Rr][Ii][Mm];
TRACE       : [Tt][Rr][Aa][Cc][Ee];
ERROR       : [Ee][Rr][Rr][Oo][Rr];
WARNING     : [Ww][Aa][Rr][Nn][Ii][Nn][Gg];
FATAL       : [Ff][Aa][Tt][Aa][Ll];
DEBUG       : [Dd][Ee][Bb][Uu][Gg];
IOCC        : [Ii][Oo][Cc][Cc];
NOCC        : [Nn][Oo][Cc][Cc];
BREAK       : [Bb][Rr][Ee][Aa][Kk];

PLUS        : '+';
MINUS       : '-';
STAR        : '*';
SLASH       : '/';
EQUALS      : '=';
NOTEQUALS   : '<>';
LPAREN      : '(';
RPAREN      : ')';
COMMA       : ',';
SEMICOLON   : ';';
HASH        : '$';
PERCENT     : '%';
COLON       : ':';
TILDA       : '~';
BANG        : '!';
MORE        : '>';
MOREEQ      : '>=';
LESS        : '<';
LESSEQ      : '<=';

fragment DIGIT: [0-9];
fragment INTEGER: DIGIT+;
fragment ALNUM: [0-9a-zA-Z];

FORMATEXIT
            : [&] [A-Za-z] [A-Za-z0-9]*
            ;

FLOAT
        : '-'? DIGIT+ ('.' DIGIT+)?
        ;

SIGNED      : '-'? INTEGER;
UNSIGNED: [0-9]+;

//============================================================
// ПРОБЕЛЫ И КОММЕНТАРИИ
//============================================================

WS          : [ \t\r\n\u000C]+ -> skip;

COMMENT
        : '/*' .*? '\n' -> skip
        ;


