//============================================================
// УПРОЩЕННАЯ ГРАММАТИКА ЯЗЫКА ФОРМАТИРОВАНИЯ ИРБИС
// грамматика для ANTLR 4.2
// Автор: А. В. Миронов
// Версия: 0.0.21
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
        : formatElement+
        ;

//============================================================
// ОБЫЧНОЕ ФОРМАТИРОВАНИЕ
//============================================================

formatElement
        : conditionalStatement              # ConditionalStatementOuter
        | leftHand
            FIELD
            rigthHand                       # FieldReference
        | FORMATEXIT
            LPAREN
            statement*
            RPAREN                          # FormatExit
        | ERROR
            LPAREN
            statement*
            RPAREN                          # ErrorFunction
        | WARNING
            LPAREN
            statement*
            RPAREN                          # WarningFunction
        | FATAL
            LPAREN
            statement*
            RPAREN                          # FatalFunction
        | TRACE
            LPAREN
            statement*
            RPAREN                          # TraceFunction
        | DEBUG
            LPAREN
            statement*
            RPAREN                          # DebugFunction
        | BANG                              # DebugBreak
        | S
            LPAREN
            statement*
            RPAREN                          # SFunction
        | F
            LPAREN
            arg1=arithExpr
            ( COMMA arg2=arithExpr
                ( COMMA arg3=arithExpr )?
            )?
            RPAREN                          # FFunction
        | REF
            LPAREN
            arg1=arithExpr
            COMMA
            arg2=statement*
            RPAREN                          # RefFunction
        | TRIM
            LPAREN
            statement*
            RPAREN                          # TrimFunction
        | IOCC                              # IoccOperator
        | NOCC                              # NoccOperator
        | NOCC
            LPAREN
            FIELD
            RPAREN                          # NoccField
        | LITERALQUOTE                      # LiteralQuote
        | ESCAPED                           # EscapedLiteral
        | BREAK                             # BreakOperator
        | GLOBALVAR                         # GlobalReference
        | UNCONDITIONAL                     # UnconditionalLiteral
        | MFN                               # SimpleMfn
        | MFNWITHLENGTH                     # MfnWithLength
        | COMMA                             # CommaOperator
        | SLASH                             # SlashOperator
        | HASH                              # HashOperator
        | PERCENT                           # PercentOperator
        | MODESWITCH                        # ModeSwitch
        | COMMANDC                          # CommandC
        | COMMANDX                          # CommandX
        | 'getenv'
            LPAREN
            name=UNCONDITIONAL
            RPAREN                          # GetEnvFunction // CISIS
        | 'putenv'
            LPAREN
            name=UNCONDITIONAL
            COMMA
            theValue=statement*
            RPAREN                          # PutEnvFunction // CISIS
        | 'system'
            LPAREN
            command=statement*
            RPAREN                          # SystemFunction // CISIS
        | 'host'                            # HostFunction
        | 'port'                            # PortFunction
        | 'serverVersion'                   # ServerVersionFunction
        | 'clientVersion'                   # ClientVersionFunction
        | 'organization'                    # OrganizationFunction
        | 'mstname'                         # MstNameFunction // CISIS
        | 'database'                        # DatabaseFunction
        | 'user'                            # UserFunction
        | 'requireServer'
            LPAREN
            version=UNCONDITIONAL
            RPAREN                          # RequireServerFunction
        | 'requireClient'
            LPAREN
            version=UNCONDITIONAL
            RPAREN                          # RequireClientFunction
        | 'left'
            LPAREN
            text=statement
            COMMA
            len=UNSIGNED
            RPAREN                          # LeftFunction // CISIS
        | 'mid'
            LPAREN
            text=statement*
            COMMA
            offset=arithExpr
            COMMA
            len=arithExpr
            RPAREN                          # MidFunction // CISIS
        | 'right'
            LPAREN
            text=statement
            COMMA
            len=UNSIGNED
            RPAREN                          # RightFunction // CISIS
        | 'trimleft'
            LPAREN
            text=statement
            RPAREN                          # TrimLeftFunction
        | 'trimright'
            LPAREN
            text=statement
            RPAREN                          # TrimRightFunction
        | 'pad'
            LPAREN
            text=statement
            COMMA
            len=arithExpr
            RPAREN                          # PadFunction
        | 'padleft'
            LPAREN
            text=statement
            COMMA
            len=arithExpr
            RPAREN                          # PadLeftFunction
        | 'padright'
            LPAREN
            text=statement
            COMMA
            len=arithExpr
            RPAREN                          # PadRightFunction
        | 'msg'
            LPAREN
            index=arithExpr
            RPAREN                          # MsgFunction
        | 'chr'
            LPAREN
            code=arithExpr
            RPAREN                          # ChrFunction
        | 'ord'
            LPAREN
            text=statement*
            RPAREN                          # OrdFunction
        | 'tolower'
            LPAREN
            text=statement*
            RPAREN                          # ToLowerFunction
        | 'toupper'
            LPAREN
            text=statement*
            RPAREN                          # ToUpperFunction
        | 'replace'
            LPAREN
            arg1=statement*
            COMMA
            arg2=statement*
            COMMA
            arg3=statement*
            RPAREN                          # ReplaceFunction // CISIS
        | 'type'
            LPAREN
            text=statement*
            RPAREN                          # TypeFunction // CISIS
        | 'include'
            LPAREN
            text=statement*
            RPAREN                          # IncludeFunction
        | INCLUSION                         # IncludeStatement // CISIS
        | 'cat'
            LPAREN
            text=statement*
            RPAREN                          # CatFunction // CISIS
        | 'date'                            # SimpleDate // CISIS
        | 'date'
            LPAREN
            text=statement*
            RPAREN                          # FormattedDate // CISIS
        | 'now'                             # NowFunction
        | 'time'                            # SimpleTime
        | 'time'
            LPAREN
            text=statement*
            RPAREN                          # FormattedTime
        | 'proc'
            LPAREN
            text=statement*
            RPAREN                          # ProcFunction // CISIS
        | 'message'
            LPAREN
            text=statement*
            RPAREN                          # MessageFunction
        | 'ask'
            LPAREN
            text=statement*
            RPAREN                          # AskFunction
        | 'beep'                            # BeepFunction
        ;

leftHand
        :
            (REPEATABLE PLUS?|CONDITIONAL)*
        ;

rigthHand
        :
            (PLUS? REPEATABLE|CONDITIONAL)*
        ;

//============================================================
// УСЛОВНЫЙ ОПЕРАТОР
//============================================================

conditionalStatement
        : IF condition
          THEN thenBranch=statement*
          ( ELSE elseBranch=statement* )?
          FI
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
        : left=formatElement
          op=(COLON|EQUALS|NOTEQUALS|MORE|MOREEQ|LESS|LESSEQ)
          right=formatElement
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
        | SIGNED                        # IntegerValue
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
        | 'npost'
            LPAREN
            FIELD
            RPAREN                      # NpostFunction // CISIS
        | 'size'
            LPAREN
            text=nonGrouped
            RPAREN                      # SizeFunction // CISIS
        ;

fieldPresense
        : P LPAREN FIELD RPAREN
        | A LPAREN FIELD RPAREN
        ;

//============================================================
// ТЕРМИНАЛЫ
//============================================================

SIGNED
            : '-'? [0-9]+
            ;

UNSIGNED    : [0-9]+
            ;

FORMATEXIT
            : [&] [A-Za-z] [A-Za-z0-9]*
            ;

FLOAT
            : '-'? [0-9]+ ('.' [0-9]+)?
            ;


LITERALQUOTE
            : '<<<' .*? '>>>'
            ;

ESCAPED
            : '`' .*? '`'
            ;

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
            : [dvn] [0-9]+
                ( '@' [0-9]+ )?
                ( '^' [A-Za-z0-9*] )?
                ( '[' ([0-9]+ | LAST) (MINUS [0-9]+)? ']')?
                ( '*' [0-9]+ )?
                ( '.' [0-9]+ )?
            ;

GLOBALVAR
            : 'g' [0-9]+
                ( '^' [A-Za-z0-9*] )?
                ( '*' [0-9]+ )?
                ( '.' [0-9]+ )?
            ;

MODESWITCH
            : [Mm][PpHhDd][UuLl]
            ;

MFNWITHLENGTH
            : [Mm][Ff][Nn][(][0-9]+[)]
            ;

INCLUSION
            :  '@' [A-Za-z0-9_.]+
            ;

COMMANDC    :
               [Cc] UNSIGNED
            ;

COMMANDX    :   [Xx]
                UNSIGNED
            ;

A           : [Aa];
AND         : [Aa][Nn][Dd];
AT          : '@';
BANG        : '!';
BREAK       : [Bb][Rr][Ee][Aa][Kk];
COLON       : ':';
COMMA       : ',';
DEBUG       : [Dd][Ee][Bb][Uu][Gg];
ELSE        : [Ee][Ll][Ss][Ee];
EQUALS      : '=';
ERROR       : [Ee][Rr][Rr][Oo][Rr];
F           : [Ff];
FATAL       : [Ff][Aa][Tt][Aa][Ll];
FI          : [Ff][Ii];
HASH        : '#';
IF          : [Ii][Ff];
IOCC        : [Ii][Oo][Cc][Cc];
L           : [Ll];
LAST        : 'LAST';
LESS        : '<';
LESSEQ      : '<=';
LPAREN      : '(';
MFN         : [Mm][Ff][Nn];
MINUS       : '-';
MORE        : '>';
MOREEQ      : '>=';
NOCC        : [Nn][Oo][Cc][Cc];
NOT         : [Nn][Oo][Tt];
NOTEQUALS   : '<>';
OR          : [Oo][Rr];
P           : [Pp];
PERCENT     : '%';
PLUS        : '+';
RAVR        : [Rr][Aa][Vv][Rr];
REF         : [Rr][Ee][Ff];
RMAX        : [Rr][Mm][Aa][Xx];
RMIN        : [Rr][Mm][Ii][Nn];
RPAREN      : ')';
RSUM        : [Rr][Ss][Uu][Mm];
S           : [Ss];
SEMICOLON   : ';';
SLASH       : '/';
STAR        : '*';
THEN        : [Tt][Hh][Ee][Nn];
TILDA       : '~';
TRACE       : [Tt][Rr][Aa][Cc][Ee];
TRIM        : [Tt][Rr][Ii][Mm];
VAL         : [Vv][Aa][Ll];
WARNING     : [Ww][Aa][Rr][Nn][Ii][Nn][Gg];


//============================================================
// ПРОБЕЛЫ И КОММЕНТАРИИ
//============================================================

WS          : [ \t\r\n\u000C]+ -> skip;

COMMENT
            : '/*' .*? ('\n' | '*/') -> skip
            ;


