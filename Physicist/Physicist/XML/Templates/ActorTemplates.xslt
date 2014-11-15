<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
  xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
  xmlns:system="SystemPrefabs"
  xmlns:physicist="PhysicistTypes"
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

  <xsl:template match="physicist:Actors/system:Mario">
    <Player class="Player">
      <xsl:call-template name="Actor_Template">     
        <xsl:with-param name="BodyInfo">
          <BodyInfo>
            <Rectangle width="19" height="40"  density="1" bodyType="Dynamic" fixedRotation="true" friction="1">
              <xsl:copy-of select="system:Position"/>
            </Rectangle>
          </BodyInfo>
        </xsl:with-param>
        
        <xsl:with-param name="GameSprites">
          <GameSprite spriteName="mario" textureRef="{@textureRef}" frameLength="0.2" depth="0">
            <Offset x="0" y="0"/>
            <FrameSize width="19" height="40"/>
            <Animations>
              <Animation name="Idle" struct="SpriteAnimation" rowIndex="0" frameCount="1" defaultFrameRate="1" playInReverse="false" flipVertical="false" flipHorizontal="false"/>
              <Animation name="Down" struct="SpriteAnimation" rowIndex="0" frameCount="8" defaultFrameRate="1" playInReverse="false" flipVertical="false" flipHorizontal="false"/>
              <Animation name="Up" struct="SpriteAnimation" rowIndex="0" frameCount="8" defaultFrameRate="1" playInReverse="false" flipVertical="true" flipHorizontal="false"/>
              <Animation name="Right" struct="SpriteAnimation" rowIndex="1" frameCount="8" defaultFrameRate="1" playInReverse="false" flipVertical="false" flipHorizontal="false"/>
              <Animation name="Left" struct="SpriteAnimation" rowIndex="1" frameCount="8" defaultFrameRate="1" playInReverse="false" flipVertical="false" flipHorizontal="true"/>
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
        
      </xsl:call-template>
    </Player>
  </xsl:template>

  <xsl:template name="Actor_Template">
    <xsl:param name="BodyInfo"/>
    <xsl:param name="TextureRef"/>
    <xsl:param name="Health"/>
    <xsl:param name="MovementSpeed"/>
    <xsl:param name="GameSprites"/>
      <Actor class="Actor" health="{$Health}" rotation="0" isEnabled="true" visibleState="Visible">
        <xsl:copy-of select="$MovementSpeed"/>
        <Sprites>
          <xsl:copy-of select="$GameSprites"/>
        </Sprites>
        <xsl:copy-of select="$BodyInfo"/>
      </Actor>
  </xsl:template>
</xsl:stylesheet>
