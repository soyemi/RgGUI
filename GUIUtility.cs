﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using System.Runtime.CompilerServices;


namespace Rigel.GUI
{
    public static class GUIUtility
    {
        /// <summary>
        /// rect is relative to group
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="group"></param>
        /// <returns>**absolute rect**</returns>
        internal static bool RectClip(ref Vector4 rect, Vector4 group, bool intersectChecked = false)
        {
            Vector2 rb = rect.Pos() + rect.Size();

            if (!intersectChecked)
            {
                if (rb.X < 0 || rb.Y < 0) return false;
                if (rect.X > group.Z || rect.Y > group.W) return false;
            }

            rect.X = Mathf.Clamp(rect.X, 0, group.Z);
            rect.Y = Mathf.Clamp(rect.Y, 0, group.W);
            rb.X = Mathf.Clamp(rb.X, rect.X, group.Z);
            rb.Y = Mathf.Clamp(rb.Y, rect.Y, group.W);

            rect.Z = rb.X - rect.X;
            rect.W = rb.Y - rect.Y;

            rect.X += group.X;
            rect.Y += group.Y;

            if (rect.Z < 1.0f || rect.W < 1.0f) return false;
            return true;
        }

        internal static bool RectClipAbosolute(ref Vector4 recta,Vector4 group, bool intersectChecked = false)
        {
            recta.x -= group.x;
            recta.y -= group.y;
            return RectClip(ref recta, group, intersectChecked);
        }

        internal static bool RectClip(ref Vector4 rect, Vector4 group, out Vector4 clipoffset,bool intersectChecked = false)
        {
            clipoffset = Vector4.Zero;
            var rectori = rect;

            Vector2 rb = rect.Pos() + rect.Size();
            if (!intersectChecked)
            {
                if (rb.X < 0 || rb.Y < 0) return false;
                if (rect.X > group.Z || rect.Y > group.W) return false;
            }

            rect.X = Mathf.Clamp(rect.X, 0, group.Z);
            rect.Y = Mathf.Clamp(rect.Y, 0, group.W);
            rb.X = Mathf.Clamp(rb.X, rect.X, group.Z);
            rb.Y = Mathf.Clamp(rb.Y, rect.Y, group.W);

            rect.Z = rb.X - rect.X;
            rect.W = rb.Y - rect.Y;

            clipoffset = rectori - rect;

            rect.X += group.X;
            rect.Y += group.Y;

            if (rect.Z < 1.0f || rect.W < 1.0f) return false;

            return true;
        }

        internal static bool RectContainsCheck(Vector2 pos, Vector2 size, Vector2 point)
        {
            if (point.X < pos.X || point.X > pos.X + size.X) return false;
            if (point.Y < pos.Y || point.Y > pos.Y + size.Y) return false;
            return true;
        }

        internal static bool RectContainsCheck(Vector4 rect, Vector2 point)
        {
            if (point.X < rect.X || point.X > rect.X + rect.Z) return false;
            if (point.Y < rect.Y || point.Y > rect.Y + rect.W) return false;
            return true;
        }

        internal static bool RectIntersectCheck(Vector4 rect, Vector4 rectrelative)
        {
            if (rectrelative.x >= rect.z) return false;
            if (rectrelative.y >= rect.w) return false;
            if (rectrelative.x  <= -rectrelative.z) return false;
            if (rectrelative.y  <= -rectrelative.w) return false;
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float rectx2(this Vector4 r)
        {
            return r.x + r.z;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float recty2(this Vector4 r)
        {
            return r.y + r.w;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 Padding(this Vector4 v, float offset)
        {
            v.X += offset;
            v.Y += offset;
            v.Z -= offset * 2;
            v.W -= offset * 2;
            return v;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 Move(this Vector4 v, Vector2 off)
        {
            v.X += off.X;
            v.Y += off.Y;
            return v;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 Truncate(this Vector4 v)
        {
            v.x = (int)v.x;
            v.y = (int)v.y;
            v.z = (int)v.z;
            v.w = (int)v.w;

            return v;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Truncate(this Vector2 v)
        {
            v.x = (int)v.x;
            v.y = (int)v.y;
            return v;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Truncate(this Vector3 v)
        {
            v.x = (int)v.x;
            v.y = (int)v.y;
            v.z = (int)v.z;
            return v;
        }

        public static Vector4 CenterPos(this Vector4 v, Vector2 size)
        {
            size = (size - v.Size()) * 0.5f;
            v.X += (int)size.X;
            v.Y += (int)size.Y;

            return v;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 Move(this Vector4 v, float offx, float offy)
        {
            v.X += offx;
            v.Y += offy;
            return v;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 SetSize(this Vector4 v, Vector2 size)
        {
            v.Z += size.X;
            v.W += size.Y;
            return v;
        }

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 SetPos(this Vector4 v, Vector2 pos)
        {
            v.x = pos.x;
            v.y = pos.y;
            return v;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 SetSize(this Vector4 v, float szx, float szy)
        {
            v.Z = szx;
            v.W = szy;
            return v;
        }

        public static long GetHash(Vector4 rect, GUIObjType type)
        {
            MemoryStream ms = new MemoryStream(20);
            BinaryWriter bw = new BinaryWriter(ms);
            bw.Write(rect.X);
            bw.Write(rect.Y);
            bw.Write(rect.Z);
            bw.Write(rect.W);
            bw.Write((byte)type);

            long hash = Rigel.Algorithms.HashFunction.RSHash(ms.ToArray());

            bw.Close();
            ms.Dispose();

            return hash;
        }

        public static long GetHash(int hashcode,Vector4 rect, GUIObjType type)
        {
            MemoryStream ms = new MemoryStream(24);
            BinaryWriter bw = new BinaryWriter(ms);
            bw.Write(hashcode);
            bw.Write(rect.X);
            bw.Write(rect.Y);
            bw.Write(rect.Z);
            bw.Write(rect.W);
            bw.Write((byte)type);

            long hash = Rigel.Algorithms.HashFunction.RSHash(ms.ToArray());

            bw.Close();
            ms.Dispose();

            return hash;
        }

    }
}
