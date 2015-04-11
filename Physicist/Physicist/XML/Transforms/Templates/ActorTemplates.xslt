<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
  xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
  xmlns:system="SystemPrefabs"
  xmlns:physicist="PhysicistTypes"
  xmlns:actor="PhysicistActors"
>
  <xsl:output method="xml" indent="yes"/>

  <xsl:template match="@*|node()">
    <xsl:copy>
      <xsl:apply-templates select="@* | node()"/>
    </xsl:copy>
  </xsl:template>

  <xsl:template name="DefaultParameter">
    <xsl:param name="Input"/>
    <xsl:param name="DefaultValue"/>
    <xsl:param name="Type"/>
    <xsl:choose>
      <xsl:when test="$Input!=''">
        <xsl:choose>
          <xsl:when test="string($Type)='Attribute'">
            <xsl:value-of select="$Input"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:copy-of select="$Input"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:when>
      <xsl:otherwise>
        <xsl:choose>
          <xsl:when test="string($Type)='Attribute'">
            <xsl:value-of select="$DefaultValue"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:copy-of select="$DefaultValue"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
            
    <xsl:template match="actor:Ticker">
    <Ticker width="{@width}" height="{@height}" transitTime="{@transitTime}" messageDelay="{@messageDelay}" fontRef="{@fontRef}">
      <xsl:copy-of select="physicist:Messages"/>
      <xsl:call-template name="Actor_Template">
        <xsl:with-param name="Name" select="@name"/>
        <xsl:with-param name="BodyInfo">
          <BodyInfo>
            <Rectangle height="{@height}" width="{@width}" density="1" bodyType="Static" fixedRotation="false" friction="0">
              <xsl:copy-of select="physicist:Position"/>
            </Rectangle>
          </BodyInfo>
        </xsl:with-param>

        <xsl:with-param name="GameSprites">
          <GameSprite spriteName="Ticker" textureRef="{@tickerBackgroundTextureRef}" depth="0.5">
            <FrameSize height="{@height}" width="{@width}"/>
            <Offset x="0" y="0"/>
            <Animations>
              <Animation name="Idle" defaultFrameRate="1" frameCount="1" rowIndex="0"/>
            </Animations>
          </GameSprite>
        </xsl:with-param>

        <xsl:with-param name="Health">
          <xsl:call-template name="DefaultParameter">
            <xsl:with-param name="Input" select="@health"/>
            <xsl:with-param name="DefaultValue" select="10"/>
            <xsl:with-param name="Type" select='"Attribute"'/>
          </xsl:call-template>
        </xsl:with-param>

        <xsl:with-param name="MovementSpeed">
          <MovementSpeed x="0" y="0"/>
        </xsl:with-param>

        <xsl:with-param name="Damage">
          <xsl:text disable-output-escaping="yes">false</xsl:text>
        </xsl:with-param>
        
      </xsl:call-template>
    </Ticker>
  </xsl:template>

  <xsl:template match="actor:Door">
    <Door targetDoor="{@targetDoor}">
      <xsl:call-template name="Actor_Template">
        <xsl:with-param name="Name" select="@name"/>
        <xsl:with-param name="BodyInfo">
          <BodyInfo>
            <Rectangle height="112" width="70" density="1" bodyType="Static" fixedRotation="false" friction="0">
              <xsl:copy-of select="physicist:Position"/>
            </Rectangle>
          </BodyInfo>
        </xsl:with-param>

        <xsl:with-param name="GameSprites">
          <GameSprite spriteName="Door" textureRef="{@doorTextureReference}" depth="0.5">
            <FrameSize height="112" width="70"/>
            <Offset x="0" y="0"/>
            <Animations>
              <Animation name="Idle" defaultFrameRate="1" frameCount="1" rowIndex="0"/>
              <Animation name="Open" defaultFrameRate="0.05" frameCount="7" loopAnimation="false" rowIndex="1" />
              <Animation name="Close" defaultFrameRate="0.05" frameCount="7" loopAnimation="false" rowIndex="1" playInReverse="true"/>
            </Animations>
          </GameSprite>
          <GameSprite spriteName="Interior" textureRef="{@interiorTextureReference}" depth="0.3">
            <FrameSize height="112" width="70"/>
            <Offset x="0" y="0"/>
            <Animations>
              <Animation name="Idle" defaultFrameRate="0.2" frameCount="1" rowIndex="0"/>
            </Animations>
          </GameSprite>
        </xsl:with-param>

        <xsl:with-param name="Health">
          <xsl:call-template name="DefaultParameter">
            <xsl:with-param name="Input" select="@health"/>
            <xsl:with-param name="DefaultValue" select="10"/>
            <xsl:with-param name="Type" select='"Attribute"'/>
          </xsl:call-template>
        </xsl:with-param>

        <xsl:with-param name="MovementSpeed">
          <MovementSpeed x="0" y="0"/>
        </xsl:with-param>

        <xsl:with-param name="Damage">
          <xsl:text disable-output-escaping="yes">false</xsl:text>
        </xsl:with-param>
        
      </xsl:call-template>
    </Door>
  </xsl:template>

  <xsl:template match="system:Mario">
    <Player class="Player" rotateSoundRef ="{@rotateSoundRef}">
      <xsl:call-template name="Actor_Template">     
        <xsl:with-param name="BodyInfo">
          <BodyInfo>
            <Rectangle width="19" height="40" density="1" bodyType="Dynamic" fixedRotation="true" friction="1">
              <xsl:copy-of select="system:Position"/>
            </Rectangle>
          </BodyInfo>
        </xsl:with-param>
                
        <xsl:with-param name="GameSprites">
          <GameSprite spriteName="mario" textureRef="{@textureRef}" frameLength="0.2" depth="0.8">
            <Offset x="0" y="0"/>
            <FrameSize width="19" height="40"/>
            <Animations>
              <Animation name="Idle" struct="SpriteAnimation" rowIndex="0" frameCount="1" defaultFrameRate="0.2" playInReverse="false" flipVertical="false" flipHorizontal="false"/>
              <Animation name="Down" struct="SpriteAnimation" rowIndex="0" frameCount="8" defaultFrameRate="0.2" playInReverse="false" flipVertical="false" flipHorizontal="false"/>
              <Animation name="Up" struct="SpriteAnimation" rowIndex="0" frameCount="8" defaultFrameRate="0.2" playInReverse="false" flipVertical="true" flipHorizontal="false"/>
              <Animation name="Right" struct="SpriteAnimation" rowIndex="1" frameCount="8" defaultFrameRate="0.2" playInReverse="false" flipVertical="false" flipHorizontal="false"/>
              <Animation name="Left" struct="SpriteAnimation" rowIndex="1" frameCount="8" defaultFrameRate="0.2" playInReverse="false" flipVertical="false" flipHorizontal="true"/>
            </Animations>
          </GameSprite>
        </xsl:with-param>

        <xsl:with-param name="Health">
          <xsl:call-template name="DefaultParameter">
            <xsl:with-param name="Input" select="@health"/>
            <xsl:with-param name="DefaultValue" select="10"/>
            <xsl:with-param name="Type" select='"Attribute"'/>
          </xsl:call-template>
        </xsl:with-param>

        <xsl:with-param name="MovementSpeed">
          <xsl:call-template name="DefaultParameter">
            <xsl:with-param name="Input" select="MovementSpeed"/>
            <xsl:with-param name="DefaultValue">              
              <MovementSpeed x="5" y="5"/>
            </xsl:with-param>
          </xsl:call-template>
        </xsl:with-param>
        
        <xsl:with-param name="Damage">
          <xsl:text disable-output-escaping="yes">true</xsl:text>
        </xsl:with-param>

      </xsl:call-template>
    </Player>
  </xsl:template>

  <xsl:template name="Actor_Template">
    <xsl:param name="BodyInfo"/>
    <xsl:param name="Health"/>
    <xsl:param name="MovementSpeed"/>
    <xsl:param name="GameSprites"/>
    <xsl:param name="Damage"/>
    <xsl:param name="Name"/>
      <Actor class="Actor" name="{$Name}" health="{$Health}" rotation="0" isEnabled="true" visibleState="Visible" canBeDamaged="{$Damage}">
        <xsl:copy-of select="$MovementSpeed"/>
        <Sprites>
          <xsl:copy-of select="$GameSprites"/>
        </Sprites>
        <xsl:copy-of select="$BodyInfo"/>
      </Actor>
  </xsl:template>
</xsl:stylesheet>
